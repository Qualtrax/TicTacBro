using System;
using System.Collections.Generic;
using System.Linq;
using TicTacBro.Domain.Events;
using TicTacBro.Domain.WinConditions;
using TicTacBro.Factories;

namespace TicTacBro.Domain
{
    public class Game : AggregateRoot
    {
        private IPlayer lastPlayer;
        private IPlayer[] playerStates;
        private List<IWinCondition> winConditions;

        public Game()
        {
            lastPlayer = new PlayerNone();
            var factory = new NewGameFactory();
            playerStates = factory.BuildEmptyGame().ToArray();
            winConditions = new List<IWinCondition>()
            {
                new RowOneWinCondition(),
                new RowTwoWinCondition(),
                new RowThreeWinCondition(),
                new ColumnOneWinCondition(),
                new ColumnTwoWinCondition(),
                new ColumnThreeWinCondition(),
                new DiagonalTopLeftWinCondition(),
                new DiagonalTopRightWinCondition()
            };
        }

        public void MakeMove(IPlayer player, Int32 position)
        {
            if (lastPlayer.Identification() == player.Identification())
            {
                LogEvent(new MovedOutOfTurnEvent { Player = player });
                return;
            }

            lastPlayer = player;
            SetSquareStateAt(player, position);
            LogEvent(new MoveEvent { Player = player, Position = position });

            CheckForChangeInGameStatus(position);
        }

        private void SetSquareStateAt(IPlayer value, Int32 index)
        {
            ValidateIndexIsInRange(index);

            if (playerStates[index].Identification() == new PlayerNone().Identification())
                playerStates[index] = value;
            else
                throw new InvalidOperationException("State has already been set bro...");
        }

        private void ValidateIndexIsInRange(Int32 index)
        {
            if (index > 8)
                throw new ArgumentOutOfRangeException("Invalid Square State index bro...");

            if (index < 0)
                throw new ArgumentOutOfRangeException("Invalid Square State index bro...");
        }

        private void CheckForChangeInGameStatus(Int32 indexSelected)
        {
            var winConditionsToCheck = winConditions.Where(c => c.ChecksPosition(indexSelected));
            var lastPlayedState = playerStates[indexSelected];

            foreach (var winCondition in winConditionsToCheck.Reverse())
            {
                if (winCondition.IsMet(playerStates, lastPlayedState))
                {
                    if (lastPlayedState.Identification() == new PlayerX().Identification())
                        LogEvent(new PlayerXWonEvent());
                    else
                        LogEvent(new PlayerOWonEvent());

                    return;
                }
            }

            winConditions.RemoveAll(w => !w.CanBeMet(playerStates));

            if (OnlyOneWinConditionRemains() && OnlyTwoEmptySpacesRemain())
                LogEvent(new GameEndedInATieEvent());
        }

        private Boolean OnlyTwoEmptySpacesRemain()
        {
            return playerStates.Where(s => s.Identification() == new PlayerNone().Identification()).Count() == 2;
        }

        private Boolean OnlyOneWinConditionRemains()
        {
            return winConditions.Count() == 1;
        }
    }
}