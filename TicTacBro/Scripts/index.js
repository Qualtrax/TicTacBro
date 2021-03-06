﻿$(function () {

    var ticTacBro = $.connection.ticTacHub;
    var lastPlayerToMove;

    ticTacBro.client.updateGameStatus = function (index, lastPlayer, gameStatus) {
        var square = document.getElementById(index);
        square.classList.add('selected');

        if (lastPlayer == 'O') {
            square.classList.add('selected-o');
            addHoverClassesForX();
        }
        else if (lastPlayer == 'X') {
            square.classList.add('selected-x');
            addHoverClassesForO();
        }

        lastPlayerToMove = lastPlayer;
        checkForWin(gameStatus);
    };

    ticTacBro.client.initializeBoard = function (boardStates) {

        boardStates.forEach(function (square, index) {
            var element = document.getElementById(index);
            if (!element.classList.contains('selected') && square !== 'N') {
                element.classList.add('selected');

                if (square == 'O')
                    element.classList.add('selected-o');
                else if (square == 'X')
                    element.classList.add('selected-x');
            }
        });

        var selected = boardStates.filter(function (square) {
            return square != 'N';
        });

        var turnO = selected.length % 2 == 1;
        if (turnO)
            addHoverClassesForO();
        else
            addHoverClassesForX();
    };

    function checkForWin(state) {
        if (state == 0)
            return;

        if (state == 1)
            alert('O Has Won the Game');
        else if (state == 2)
            alert('X Has Won the Game');
        else if (state == 3)
            alert('A Tie has occured');

        removeClassFromElementList();
        ticTacBro.server.start();
    };

    function addHoverClassesForX() {
        var squares = document.getElementsByClassName('square');

        [].forEach.call(document.getElementsByClassName('square'), function (square) {
            square.classList.remove('turn-o');
            if (!square.classList.contains('selected'))
                square.classList.add('turn-x');
        });
    }

    function addHoverClassesForO() {
        var squares = document.getElementsByClassName('square');

        [].forEach.call(document.getElementsByClassName('square'), function (square) {
            square.classList.remove('turn-x');
            if (!square.classList.contains('selected'))
                square.classList.add('turn-o');
        });
    }

    function removeClassFromElementList() {
        var elementsWithClass = document.getElementsByClassName("square");

        for (var j = 0; j < elementsWithClass.length; j++)
            elementsWithClass[j].className = "square";
    }

    $.connection.hub.start().done(function () {
        ticTacBro.server.join();

        [].forEach.call(document.getElementsByClassName('square'), function (square) {
            square.addEventListener("click", function () {
                ticTacBro.server.yourTurnBro(this.id);
            });
        });
    });
});