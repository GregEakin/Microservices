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
// SUBSYSTEM: ShoppingCartSvcTests
// FILE:  EventStoreTests.cs
// AUTHOR:  Greg Eakin

using NUnit.Framework;
using ShoppingCartSvc.EventFeed;
using System.Linq;
using NUnit.Framework.Legacy;

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
            ClassicAssert.AreEqual(1, events.Length);
            ClassicAssert.AreEqual(event2, events[0].SequenceNumber);
            ClassicAssert.AreEqual("Event2", events[0].Name);
        }
    }
}