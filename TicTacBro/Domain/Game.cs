using System;
using System.Collections.Generic;
using System.Linq;
using TicTacBro.Domain.Events;
using TicTacBro.Domain.WinConditions;

namespace TicTacBro.Domain
{
    public class Game : AggregateRoot
    {
        private IPlayer lastPlayer;
        private IPlayer[] playerStates;
        private IList<IWinCondition> winConditions;

        public IEnumerable<IPlayer> States
        {
            get
            {
                return playerStates.ToList();
            }
        }

        public Game()
        {
            lastPlayer = new PlayerNone();
            playerStates = Enumerable.Select(new IPlayer[9], p => new PlayerNone()).ToArray<IPlayer>();
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
            if (lastPlayer.Type() == player.Type())
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

            if (playerStates[index].Type() == new PlayerNone().Type())
                playerStates[index] = value;
            else
                throw new InvalidOperationException("State has already been set bro...");
        }

        private void ValidateIndexIsInRange(Int32 index)
        {
            if (index > 8 || index < 0)
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
                    if (lastPlayedState.Type() == new PlayerX().Type())
                        LogEvent(new PlayerXWonEvent());
                    else
                        LogEvent(new PlayerOWonEvent());

                    return;
                }

                if (!winCondition.CanBeMet(playerStates))
                    winConditions.Remove(winCondition);
            }

            if (!winConditions.Any())
                LogEvent(new GameEndedInATieEvent());
        }
    }
}