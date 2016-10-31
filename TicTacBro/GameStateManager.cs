using System;
using TicTacBro.Domain;
using TicTacBro.Models;

namespace TicTacBro
{
    public static class GameStateManager
    {
        private static Boolean isXsTurn;
        private static Game game;

        public static void StartNewGame()
        {
            isXsTurn = true;
            GameValidator.ResetValidator();
            game = new Game();
        }

        public static void MakeMove(Int32 index)
        {
            var playerState = isXsTurn ? SquareStates.X : SquareStates.O;

            game.SetSquareStateAt(index, playerState);
            GameValidator.Validate(game, index);
            isXsTurn = !isXsTurn;
        }

        public static Boolean GameInProgress()
        {
            if (game == null)
                return false;

            return game.Status == GameStatus.Incomplete;
        }

        public static GameStatus GetGameStateStatus()
        {
            return game.Status;
        }

        public static SquareStates[] GetBoardStates()
        {
            return game.States;
        }
    }
}