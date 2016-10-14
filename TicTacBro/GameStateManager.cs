using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicTacBro.Domain;
using TicTacBro.Models;

namespace TicTacBro
{
    public static class GameStateManager
    {
        private static Boolean isXsTurn;
        private static GameState gameState;

        public static void StartNewGame()
        {
            isXsTurn = true;
            gameState = new GameState();
        }

        public static GameState MakeMove(Int32 index)
        {
            var playerState = isXsTurn ? SquareStates.X : SquareStates.O;

            gameState.SetSquareStateAt(index, playerState);
            GameValidator.Validate(gameState, index);
            isXsTurn = !isXsTurn;

            return gameState;
        }

        public static Boolean GameInProgress()
        {
            return gameState.Status == GameStatus.Incomplete;
        }

        public static GameStatus GetGameStateStatus()
        {
            return gameState.Status;
        }
    }
}