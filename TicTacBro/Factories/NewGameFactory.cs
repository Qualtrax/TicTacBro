using System.Linq;
using TicTacBro.Domain;

namespace TicTacBro.Factories
{
    public class NewGameFactory
    {
        public IPlayer[] BuildEmptyGame()
        {
            return Enumerable.Select(new IPlayer[9], p => new PlayerNone()).ToArray<IPlayer>();
        }
    }
}