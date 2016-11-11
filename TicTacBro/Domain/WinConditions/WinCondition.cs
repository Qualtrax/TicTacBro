using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacBro.Domain.WinConditions
{
    public abstract class WinCondition : IWinCondition
    {
        protected IEnumerable<Int32> condition;

        public WinCondition()
        {
            condition = Enumerable.Empty<Int32>();
        }

        public Boolean ChecksPosition(Int32 position)
        {
            return condition.Any(c => c == position);
        }

        public Boolean IsMet(IEnumerable<IPlayer> playerStates, IPlayer player)
        {
            return condition.All(c => playerStates.ElementAt(c).Type() == player.Type());
        }

        public Boolean CanBeMet(IEnumerable<IPlayer> playerStates)
        {
            return condition
                .Where(c => playerStates.ElementAt(c).Type() != new PlayerNone().Type())
                .GroupBy(c => playerStates.ElementAt(c).Type()).Count() <= 1;
        }
    }
}