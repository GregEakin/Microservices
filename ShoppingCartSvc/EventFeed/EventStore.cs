// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  EventStore.cs
// AUTHOR:  Greg Eakin

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ShoppingCartSvc.EventFeed
{
    public class EventStore : IEventStore
    {
        private static long _currentSequenceNumber = 0;
        private static readonly IList<Event> Database = new List<Event>();

        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
            => Database
                .Where(e =>
                    e.SequenceNumber >= firstEventSequenceNumber &&
                    e.SequenceNumber <= lastEventSequenceNumber)
                .OrderBy(e => e.SequenceNumber);

        public void Raise(string eventName, object content)
        {
            var seqNumber = Interlocked.Increment(ref _currentSequenceNumber);
            var @event = new Event(seqNumber, DateTime.UtcNow, eventName, content);
            Database.Add(@event);
        }
    }
}