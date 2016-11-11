using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacBro.Domain.WinConditions
{
    public class RowOneWinCondition : WinCondition
    {
        public RowOneWinCondition()
        {
            condition = new[] { 0, 1, 2 };
        }
    }
}