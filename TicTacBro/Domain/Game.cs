using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicTacBro.Models;

namespace TicTacBro.Domain
{
    public class Game
    {
        public SquareStates[] States { get; }
        public GameStatus Status { get; set; }

        public Game()
        {
            Status = GameStatus.Incomplete;
            States = new SquareStates[9];
            States = States.Select(c => { c = SquareStates.Empty; return c; }).ToArray();
        }

        public void SetSquareStateAt(Int32 index, SquareStates value)
        {
            ValidateIndex(index);

            if (States[index] == SquareStates.Empty)
                States[index] = value;
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