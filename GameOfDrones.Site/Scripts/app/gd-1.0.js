﻿/// <reference path="../jquery/jquery-3.1.1.intellisense.js" />
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
        var currentPlayer = $.data(text, 'info');

        if (text) {
            text.value = 'Player_' + currentPlayer;
        }

        var playerInfo = {
            Id: 0,
            Name: text.value,
            Handshape: 0,
            Score: 0
        };

        playersInfo.push(playerInfo);

        if (currentPlayer < config.players) {
            $('#gd-prompt-player' + currentPlayer).hide('fast');
            $('#gd-prompt-player' + (currentPlayer + 1)).show('fast');
        } else {
            $('#gd-prompt-player' + currentPlayer).hide('fast');
            apiController.savePlayers(playersInfo);
            container.append(countDownSection());
        }
    }

    var countDownSection = function () {
        container.empty();

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

        var sign = SignContainer(container, playersInfo);
        sign.start();
    }

    function onPlayersSaved(e, info) {
        playersInfo = info.resultset;
    }

    function onSignSelected(e, info) {
        var playerInfo = $.grep(playersInfo, function (e) { return e.Name == info.currentplayer.Name; })[0];
        playerInfo.Handshape = info.selection.id;
        apiController.updateRound(playerInfo);
    }

    function onScoreUpdated(e, info)
    {
        console.info(info);
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
    var currentPlayer = 0;
    var e = $.Event('signSelected');
    var self = this;

    function loadGame() {
        container.empty();
        container.show('fast');
        var h3 = $('<h3>');
        h3.text(playersInfo[currentPlayer].Name + "'s turn");
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
        $(self).trigger(e, { currentplayer: playersInfo[currentPlayer], selection: $.data(this, 'info') });
        currentPlayer++;
        if (currentPlayer >= playersInfo.length)
            currentPlayer = 0;
        container.hide('slow', loadGame);
    }

    return {
        start: loadGame
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