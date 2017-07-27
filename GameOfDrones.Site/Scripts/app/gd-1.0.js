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

    var scriptDownloaded = function () {
        config = gdSetup.config;
        apiController = ApiController(config.apiUrl);
        console.info(appName + '-Config file loaded');
        $(global).on('playersSaved', onPlayersSaved)
    }

    var scriptDownloadedFailed = function (jqxhr, settings, exception) {
        console.error(appName + '-Cannot load config file');
    };

    var init = function () {
        console.info(appName + '-initialized');

        container = $('#gd-container');

        setTimeout(function () {
            if (config) {
                startGame();
            } else {
                console.error(appName + '- Time out of ' + loadConfigTimeOut + 'ms reached for loading config file, the game wont start');
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
        var playerInfo = {
            id: 0,
            name: text.value,
            score: 0
        };
        var currentPlayer = $.data(text, 'info');

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

    function onPlayersSaved(e,info) {
        console.info('El jugador ha sido guardado');
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

    function loadGame() {
        var h3 = $('<h3>');
        h3.text('Hey!! ' + playersInfo[0].name + ' is your turn');
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
            $.data(div[0], 'info', signs[i]);

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
        //console.info($.data(this, 'info'));
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
            url: url + "/players",
            contentType: "application/json"
        }).done(function (resultset) {
            console.info(resultset);
            $(self).trigger(e, resultset);
        });
    }

    return {
        savePlayers: savePlayers
    }
};