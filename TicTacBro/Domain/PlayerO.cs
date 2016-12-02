using System;

namespace TicTacBro.Domain
{
    public class PlayerO : IPlayer
    {
        private Char identifier = 'O';

        public Char Identification()
        {
            return identifier;
        }
    }
}