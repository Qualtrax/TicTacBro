using System;
using System.Collections.Generic;
using System.Linq;
using TicTacBro.Domain.Events;
using TicTacBro.Models;

namespace TicTacBro.Domain
{
    public class Game
    {
        private IPlayer lastPlayer;
        private IPlayer[] playerStates;
        public GameStatus Status { get; private set; }
        public List<IEvent> Events { get; private set; }

        private static readonly Int32[] RowOne = { 0, 1, 2 };
        private static readonly Int32[] RowTwo = { 3, 4, 5 };
        private static readonly Int32[] RowThree = { 6, 7, 8 };
        private static readonly Int32[] ColumnOne = { 0, 3, 6 };
        private static readonly Int32[] ColumnTwo = { 1, 4, 7 };
        private static readonly Int32[] ColumnThree = { 2, 5, 8 };
        private static readonly Int32[] DiagonalTopLeft = { 0, 4, 8 };
        private static readonly Int32[] DiagonalTopRight = { 2, 4, 6 };
        private IList<Int32[]> winConditions;

        public IEnumerable<IPlayer> States
        {
            get
            {
                return playerStates.ToList();
            }
        }

        public Game()
        {
            Events = new List<IEvent>();
            lastPlayer = new PlayerNone();
            Status = GameStatus.Incomplete;
            playerStates = Enumerable.Select(new IPlayer[9], p => new PlayerNone()).ToArray<IPlayer>();
            winConditions = new List<Int32[]>()
            {
                RowOne, RowTwo, RowThree, ColumnOne, ColumnTwo, ColumnThree, DiagonalTopLeft, DiagonalTopRight
            };
        }

        public void MakeMove(IPlayer player, Int32 position)
        {
            if (lastPlayer.Type() == player.Type())
            {
                Events.Add(new MovedOutOfTurnEvent { Player = player });
                return;
            }

            lastPlayer = player;
            SetSquareStateAt(player, position);
            Events.Add(new MoveEvent { Player = player, Position = position });

            Validate(position);

            if (Status == GameStatus.OWin)
                Events.Add(new PlayerOWonEvent());
            else if (Status == GameStatus.XWin)
                Events.Add(new PlayerXWonEvent());
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

        private void Validate(Int32 indexSelected)
        {
            var winConditionsToCheck = winConditions.Where(c => c.Contains(indexSelected));
            var lastPlayedState = playerStates[indexSelected];

            for (var i = winConditionsToCheck.Count() - 1; i >= 0; i--)
            {
                if (PlayerWon(lastPlayedState, winConditionsToCheck.ElementAt(i)))
                {
                    Status = lastPlayedState.Type() == new PlayerX().Type() ? GameStatus.XWin : GameStatus.OWin;
                    return;
                }

                if (WinConditionShouldBeRemoved(winConditionsToCheck.ElementAt(i)))
                    winConditions.Remove(winConditionsToCheck.ElementAt(i));
            }

            if (winConditions.Any())
                Status = GameStatus.Incomplete;
            else
                Status = GameStatus.Tie;
        }

        private Boolean PlayerWon(IPlayer lastPlayedState, Int32[] winCondition)
        {
            return (winCondition.Count(c => playerStates[c].Type() == lastPlayedState.Type())) == 3;
        }

        private Boolean WinConditionShouldBeRemoved(Int32[] specificWinCondition)
        {
            return !specificWinCondition.Any(c => playerStates[c].Type() == new PlayerNone().Type());
        }

        public Boolean GameInProgress()
        {
            return Status == GameStatus.Incomplete;
        }
        
    }
}