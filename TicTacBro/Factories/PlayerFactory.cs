using System;
using TicTacBro.Domain;

namespace TicTacBro.Factories
{
    public static class PlayerFactory
    {
        public const String X = "X";
        public const String O = "O";

        public static IPlayer Build(String player)
        {
            if (String.Equals(player, X, StringComparison.InvariantCultureIgnoreCase))
                return new PlayerX();
            else if (String.Equals(player, O, StringComparison.InvariantCultureIgnoreCase))
                return new PlayerO();
            else
                throw new InvalidOperationException();
        }
    }
}