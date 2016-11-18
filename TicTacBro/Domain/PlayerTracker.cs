
namespace TicTacBro.Domain
{
    public class PlayerTracker : AggregateRoot
    {
        private IPlayer currentPlayer;

        public PlayerTracker(IPlayer startingPlayer)
        {
            currentPlayer = startingPlayer;
        }

        public IPlayer ChangePlayer()
        {
            if (currentPlayer.Identification() == new PlayerX().Identification())
            {
                currentPlayer = new PlayerO();
                return new PlayerO();
            }
            else
            {
                currentPlayer = new PlayerX();
                return new PlayerX();
            }

        }
    }
}