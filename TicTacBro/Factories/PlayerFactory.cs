using System;
using TicTacBro.Domain;

namespace TicTacBro.Factories
{
    public static class PlayerFactory
    {
        public static IPlayer CreatePlayer(Char player)
        {
            if (player == new PlayerX().Identification())
                return new PlayerX();
            if (player == new PlayerO().Identification())
                return new PlayerO();

            return new PlayerNone();
        }
    }
}