namespace TicTacBro.Models
{
    public class MovedOutOfTurnEvent : IEvent
    {
        public IPlayer Player { get; set; }
    }
}