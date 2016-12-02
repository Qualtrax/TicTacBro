using System;

namespace TicTacBro.Domain
{
    public class PlayerX : IPlayer
    {
        private char identifier = 'X';

        public Char Identification()
        {
            return identifier;
        }
    }
}