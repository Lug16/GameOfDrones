/// <reference path="../jquery/jquery-3.1.1.intellisense.js" />
/// <reference path="../jquery/jquery-3.1.1.js" />

(function (global) {
    'use strict';
    var $ = global.$;
    var config = null;
    var isNewGame = true;
    var container = null;
    var playersInfo = [];
    var loadConfigTimeOut = 500;//milliseconds
    var appName = 'Games of Drones';
    var startGameWait = 3;//Seconds
    var apiController = null;
    var signContainer = null;
    var currentPlayer = null;

    var scriptDownloaded = function () {
        config = gdSetup.config;
        apiController = ApiController(config.apiUrl);
        console.info(appName + '-Config file loaded');
    }

    var scriptDownloadedFailed = function (jqxhr, settings, exception) {
        console.error(appName + '-Cannot load config file');
    };

    var init = function () {
        console.info(appName + '-initialized');

        container = $('#gd-container');

        $(global).on('playersSaved', onPlayersSaved);
        $(global).on('signSelected', onSignSelected);
        $(global).on('scoreUpdated', onScoreUpdated);

        setTimeout(function () {
            if (config) {
                startGame();
            } else {
                console.error(appName + '- Time out of ' + loadConfigTimeOut + "ms reached for loading config file, the game won't start");
            }
        }, loadConfigTimeOut);
    }

    function startGame() {
        container.empty();
        playersInfo = [];

        if (isNewGame) {
            for (var i = 0; i < config.players; i++) {
                container.append(getPlayerNamePrompt(i + 1));
            }
        }
    }

    var getPlayerNamePrompt = function (number) {
        var div = $('<div>');
        div.addClass('col-md-2 col-md-offset-5');
        div.attr('id', 'gd-prompt-player' + number);
        if (number > 1) div.css('display', 'none');

        var h3 = $('<h3>');
        h3.text('Player ' + number);

        var span = $('<span>');
        span.text('Name:');

        var text = $('<input type="text">');
        text.attr('placeholder', 'Player ' + number)
        $.data(text[0], 'info', number);

        var button = $('<input type="button">');
        button.val('OK');
        button.on('click', playerOkClicked);

        div.append(h3);
        div.append(span);
        div.append(text);
        div.append(button);

        return div;
    }

    var playerOkClicked = function () {
        var text = $(this).siblings('input')[0];
        var playerindex = $.data(text, 'info');

        if (!text.value) {
            text.value = 'Player_' + playerindex;
        }

        var playerInfo = {
            Id: 0,
            Name: text.value,
            Handshape: 0,
            Turn: 0
        };

        playersInfo.push(playerInfo);

        if (playerindex < config.players) {
            $('#gd-prompt-player' + playerindex).hide('fast');
            $('#gd-prompt-player' + (playerindex + 1)).show('fast');
        } else {
            $('#gd-prompt-player' + playerindex).hide('fast');
            apiController.savePlayers(playersInfo);
            container.empty();
            container.append(countDownSection());
        }
    }

    var countDownSection = function () {
        var div = $('<div>');

        var h3 = $('<h3>');
        h3.text('The Game will start in:');

        var p = $('<p>')
        p.addClass('counter');
        p.text(startGameWait);

        div.append(h3);
        div.append(p);

        var counter = 1;
        var interval = setInterval(function () {
            p.text(startGameWait - counter);
            counter++;
            if (counter > startGameWait) {
                clearInterval(interval);
                loadGame();
            }
        }, 1000);

        return div;
    }

    var loadGame = function () {
        container.empty();
        currentPlayer = 0;
        signContainer = SignContainer(container, playersInfo);
        signContainer.loadGame(currentPlayer);
    }

    function onPlayersSaved(e, info) {
        playersInfo = info.resultset;
    }

    function onSignSelected(e, info) {
        var playerInfo = $.grep(playersInfo, function (e, i) { return e.Name == info.currentplayer.Name })[0];
        playerInfo.Handshape = info.selection.id;
        playerInfo.Turn = info.turn;
        apiController.updateRound(playerInfo);
    }

    function onScoreUpdated(e, info) {
        if (info.resultset.IdWinner) {
            var winner = $.grep(playersInfo, function (e, i) { return e.Id == info.resultset.IdWinner })[0];
            container.empty();
            container.append(getWinnerPrompt(winner.Name));
        } else {
            currentPlayer++;

            if (currentPlayer >= playersInfo.length) {
                container.hide('slow', loadGame);
                currentPlayer = 0;
            } else {
                container.hide('slow');
                signContainer.loadGame(currentPlayer);
            }
        }
    }

    function getWinnerPrompt(name) {
        var div = $('<div>');

        var p = $('<p>');
        p.attr('id', 'winner');
        p.text(name + ' is the winner');

        var button = $('<input type="button">');
        button.val('Play Again');
        button.on('click', startGame);

        div.append(p);
        div.append(button);

        return div;
    }

    $.getScript("Scripts/app/config.js")
        .done(function () { scriptDownloaded(); })
        .fail(function () { scriptDownloadedFailed(); });

    global.gd = {
        init: init
    };
})(window);

var SignContainer = function (container, playersInfo) {
    var signs = ['paper', 'rock', 'scissors'];
    var e = $.Event('signSelected');
    var self = this;
    var pindex = 0;

    function loadGame(playerIndex) {
        pindex = playerIndex;
        container.empty();
        container.show('fast');
        var h3 = $('<h3>');
        h3.text(playersInfo[playerIndex].Name + "'s turn");
        h3.addClass('gamerTitle');

        container.append(h3);
        container.append(getImages());
    }

    function getImages() {
        var divContainer = $('<div>');
        divContainer.addClass('signContainer');

        for (var i = 0; i < signs.length; i++) {
            var div = $('<div>');
            div.addClass('sign');
            $.data(div[0], 'info', { id: i + 1, sign: signs[i] });

            var img = $('<img style="width:200px">');
            img.addClass('img-responsive');
            img.attr('src', 'Content/images/' + signs[i] + '.png');

            div.on('click', onSignClicked);

            div.append(img);
            divContainer.append(div);
        }

        return divContainer;
    }

    function onSignClicked() {
        $(self).trigger(e, { currentplayer: playersInfo[pindex], selection: $.data(this, 'info'), turn: pindex });
    }

    return {
        loadGame: loadGame
    }
}

var ApiController = function (url) {
    var e = jQuery.Event("playersSaved");
    var f = jQuery.Event("scoreUpdated");

    var self = this;

    function savePlayers(data) {
        $.ajax({
            type: "POST",
            data: JSON.stringify(data),
            url: url + "/game",
            contentType: "application/json"
        }).done(function (resultset) {
            $(self).trigger(e, { resultset: resultset });
        });
    }

    function updateRound(data) {
        $.ajax({
            type: "POST",
            data: JSON.stringify(data),
            url: url + "/round",
            contentType: "application/json"
        }).done(function (resultset) {
            $(self).trigger(f, { resultset: resultset });
        });
    }

    return {
        savePlayers: savePlayers,
        updateRound: updateRound
    }
};