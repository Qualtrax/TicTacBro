
namespace TicTacBro.Domain.Events
{
    public class MovedOutOfTurnEvent : IEvent
    {
        public IPlayer Player { get; set; }
    }
}