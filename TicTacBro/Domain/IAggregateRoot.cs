using System.Collections.Generic;
using TicTacBro.Domain.Events;

namespace TicTacBro.Domain
{
    public interface IAggregateRoot
    {
        List<IEvent> Events { get; }
        void LogEvent(IEvent @event);
    }
}
