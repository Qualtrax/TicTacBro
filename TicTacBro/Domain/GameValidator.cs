using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicTacBro.Models;

namespace TicTacBro.Domain
{
    public static class GameValidator
    {
        private static Int32[] RowOne = { 0, 1, 2 };
        private static Int32[] RowTwo = { 3, 4, 5 };
        private static Int32[] RowThree = { 6, 7, 8 };
        private static Int32[] ColumnOne = { 0, 3, 6 };
        private static Int32[] ColumnTwo = { 1, 4, 7 };
        private static Int32[] ColumnThree = { 2, 5, 8 };
        private static Int32[] DiagonalTopLeft = { 0, 4, 8 };
        private static Int32[] DiagonalTopRight = { 2, 4, 6 };
        private static IList<Int32[]> winConditions = new List<Int32[]>()
        {
            RowOne, RowTwo, RowThree, ColumnOne, ColumnTwo, ColumnThree, DiagonalTopLeft, DiagonalTopRight
        };

        public static void Validate(GameState gameState, Int32 indexSelected)
        {
            var winConditionsToCheck = winConditions.Where(c => c.Contains(indexSelected));
            var lastPlayedState = gameState.States[indexSelected];

            for (var i = winConditionsToCheck.Count() - 1; i >= 0; i--)
            {
                if (PlayerWon(gameState, lastPlayedState, winConditionsToCheck.ElementAt(i)))
                {
                    gameState.Status = lastPlayedState == SquareStates.X ? GameStatus.XWin : GameStatus.OWin;
                    return;
                }

                if (WinConditionShouldBeRemoved(gameState, winConditionsToCheck.ElementAt(i)))
                    winConditions.Remove(winConditionsToCheck.ElementAt(i));
            }

            if (winConditions.Any())
                gameState.Status = GameStatus.Incomplete;
            else
                gameState.Status = GameStatus.Tie;
        }

        private static Boolean PlayerWon(GameState gameState, SquareStates lastPlayedState, Int32[] winCondition)
        {
            return (winCondition.Count(c => gameState.States[c] == lastPlayedState)) == 3;
        }

        private static Boolean WinConditionShouldBeRemoved(GameState gameState, Int32[] winCondition)
        {
            return !winCondition.Any(c => gameState.States[c] == SquareStates.Empty);
        }
    }
}