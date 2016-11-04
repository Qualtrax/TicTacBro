using System;

namespace TicTacBro.Models
{
    public class MoveEvent : IEvent
    {
        public Int32 Position { get; set; }
        public IPlayer Player { get; set; }
    }
}