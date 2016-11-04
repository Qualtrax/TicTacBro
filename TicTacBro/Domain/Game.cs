using System;
using System.Collections.Generic;
using System.Linq;
using TicTacBro.Models;

namespace TicTacBro.Domain
{
    public class Game
    {
        private Boolean isXsTurn;
        private GameValidator validator;
        private Board board;
        private SquareState[] states;
        public GameStatus Status { get; private set; }
        public List<IEvent> Events { get; private set; }

        public IEnumerable<SquareState> States
        {
            get
            {
                return states.ToList();
            }
        }

        public Game()
        {
            Events = new List<IEvent>();
        }

        public Game(GameValidator validator)
        {
            this.validator = validator;
            this.board = new Board();
            isXsTurn = true;
            Status = GameStatus.Incomplete;
            states = new SquareState[9];
            states = states.Select(c => { c = SquareState.Empty; return c; }).ToArray();
        }

        public void MakeMove(IPlayer player, Int32 position)
        {
            Events.Add(new MoveEvent { Player = player, Position = position });
            //var playerState = isXsTurn ? SquareState.X : SquareState.O;

            //SetSquareStateAt(index, playerState);
            //validator.Validate(states, Status, index);
            //isXsTurn = !isXsTurn;
        }

        public Boolean GameInProgress()
        {
            return Status == GameStatus.Incomplete;
        }

        public void SetSquareStateAt(Int32 index, SquareState value)
        {
            ValidateIndex(index);

            if (states[index] == SquareState.Empty)
                states[index] = value;
            else
                throw new InvalidOperationException("State has already been set bro...");
        }

        private void ValidateIndex(Int32 index)
        {
            if (index > 8 || index < 0)
                throw new ArgumentOutOfRangeException("Invalid Square State index bro...");
        }
    }
}