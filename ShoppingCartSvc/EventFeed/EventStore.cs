// Copyright 2020 Greg Eakin
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at:
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
        private static ulong _currentSequenceNumber;
        private readonly IList<Event> _database = new List<Event>();

        public IEnumerable<Event> GetEvents(ulong firstEventSequenceNumber, ulong lastEventSequenceNumber)
            => _database
                .Where(e =>
                    e.SequenceNumber >= firstEventSequenceNumber &&
                    e.SequenceNumber <= lastEventSequenceNumber)
                .OrderBy(e => e.SequenceNumber);

        public ulong Raise(string eventName, object content)
        {
            var seqNumber = Interlocked.Increment(ref _currentSequenceNumber);
            var @event = new Event(seqNumber, DateTime.UtcNow, eventName, content);
            _database.Add(@event);
            return seqNumber;
        }
    }
}