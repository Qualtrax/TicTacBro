﻿using System;
using TicTacBro.Models;

namespace TicTacBro.Domain.Events
{
    public class MoveEvent : IEvent
    {
        public Int32 Position { get; set; }
        public IPlayer Player { get; set; }
    }
}