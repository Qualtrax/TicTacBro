using System;
using System.Collections.Generic;

namespace TicTacBro.Domain.WinConditions
{
    public interface IWinCondition
    {
        Boolean CanBeMet(IEnumerable<IPlayer> playerStates);
        Boolean ChecksPosition(Int32 position);
        Boolean IsMet(IEnumerable<IPlayer> playerStates, IPlayer player);
    }
}