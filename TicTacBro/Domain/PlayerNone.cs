using System;

namespace TicTacBro.Domain
{
    public class PlayerNone : IPlayer
    {
        private Char identifier = 'N';

        public Char Identification()
        {
            return identifier;
        }
    }
}