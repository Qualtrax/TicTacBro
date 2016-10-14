using System;

namespace TicTacBro.Domain
{
    public class WinCondition
    {
        private Int32[] indexes;

        public WinCondition(Int32[] indexes)
        {
            this.indexes = indexes;
        }
    }
}