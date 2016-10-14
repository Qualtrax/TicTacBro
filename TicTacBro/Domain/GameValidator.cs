using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicTacBro.Models;

namespace TicTacBro.Domain
{
    public class GameValidator
    {
        private IList<Int32[]> winConditions;
        private Int32[] RowOne = { 0, 1, 2 };
        private Int32[] RowTwo = { 3, 4, 5 };
        private Int32[] RowThree = { 6, 7, 8 };
        private Int32[] ColumnOne = { 0, 3, 6 };
        private Int32[] ColumnTwo = { 1, 4, 7 };
        private Int32[] ColumnThree = { 2, 5, 8 };
        private Int32[] DiagonalTopLeft = { 0, 4, 8 };
        private Int32[] DiagonalTopRight = { 2, 4, 6 };

        public GameValidator()
        {
            winConditions = new List<Int32[]>()
            {
                RowOne, RowTwo, RowThree, ColumnOne, ColumnTwo, ColumnThree, DiagonalTopLeft, DiagonalTopRight
            };
        }

        public void Validate(GameState gameState, Int32 indexSelected)
        {
            var winConditionsToCheck = winConditions.Where(c => c.Contains(indexSelected));
            var lastPlayedState = gameState.States[indexSelected];

            foreach (var winCondition in winConditionsToCheck)
            {
                if (PlayerWon(gameState, lastPlayedState, winCondition))
                {
                    gameState.Status = lastPlayedState == SquareStates.X ? GameStatus.XWin : GameStatus.OWin;
                    return;
                }

                if (WinConditionShouldBeRemoved(gameState, winCondition))
                    winConditions.Remove(winCondition);
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