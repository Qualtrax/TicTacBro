using System;
using System.Collections.Generic;
using System.Linq;
using TicTacBro.Models;

namespace TicTacBro.Domain
{
    public class GameValidator
    {
        private Int32[] RowOne = { 0, 1, 2 };
        private Int32[] RowTwo = { 3, 4, 5 };
        private Int32[] RowThree = { 6, 7, 8 };
        private Int32[] ColumnOne = { 0, 3, 6 };
        private Int32[] ColumnTwo = { 1, 4, 7 };
        private Int32[] ColumnThree = { 2, 5, 8 };
        private Int32[] DiagonalTopLeft = { 0, 4, 8 };
        private Int32[] DiagonalTopRight = { 2, 4, 6 };
        private IList<Int32[]> winConditions;

        public GameValidator()
        {
            winConditions = new List<Int32[]>()
            {
                RowOne, RowTwo, RowThree, ColumnOne, ColumnTwo, ColumnThree, DiagonalTopLeft, DiagonalTopRight
            };
        }

        public void Validate(SquareState[] states, GameStatus status, Int32 indexSelected)
        {
            var winConditionsToCheck = winConditions.Where(c => c.Contains(indexSelected));
            var lastPlayedState = states[indexSelected];

            for (var i = winConditionsToCheck.Count() - 1; i >= 0; i--)
            {
                if (PlayerWon(states, lastPlayedState, winConditionsToCheck.ElementAt(i)))
                {
                    status = lastPlayedState == SquareState.X ? GameStatus.XWin : GameStatus.OWin;
                    return;
                }

                if (WinConditionShouldBeRemoved(states, winConditionsToCheck.ElementAt(i)))
                    winConditions.Remove(winConditionsToCheck.ElementAt(i));
            }

            if (winConditions.Any())
                status = GameStatus.Incomplete;
            else
                status = GameStatus.Tie;
        }

        private Boolean PlayerWon(SquareState[] states, SquareState lastPlayedState, Int32[] winCondition)
        {
            return (winCondition.Count(c => states[c] == lastPlayedState)) == 3;
        }

        private Boolean WinConditionShouldBeRemoved(SquareState[] states, Int32[] winCondition)
        {
            return !winCondition.Any(c => states[c] == SquareState.Empty);
        }
    }
}