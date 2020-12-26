// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCartSvcTests
// FILE:  EventStoreTests.cs
// AUTHOR:  Greg Eakin

using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using ShoppingCartSvc.EventFeed;

namespace ShoppingCartSvcTests.EventFeed
{
    public class EventStoreTests
    {
        [Test]
        public void GetEventTests()
        {
            var eventStore = new EventStore();
            var event1 = eventStore.Raise("Event1", null);
            var event2 = eventStore.Raise("Event2", null);
            var event3 = eventStore.Raise("Event3", null);

            var events = eventStore.GetEvents(event2, event2).ToArray();
            Assert.AreEqual(1, events.Count());
            Assert.AreEqual(event2, events[0].SequenceNumber);
            Assert.AreEqual("Event2", events[0].Name);
        }
    }
}