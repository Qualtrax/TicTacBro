using System;
using System.Collections.Generic;
using System.Linq;
using TicTacBro.Domain.Events;

namespace TicTacBro.Domain
{
    public class AggregateRoot : IAggregateRoot
    {
        private List<IEvent> events;
        
        public AggregateRoot()
        {
            events = new List<IEvent>();
        }

        public List<IEvent> Events
        {
            get
            {
                return events.ToList();
            }
        }

        public void LogEvent(IEvent @event)
        {
            events.Add(@event);
        }
    }
}