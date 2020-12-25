// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  IEventStore.cs
// AUTHOR:  Greg Eakin

using System.Collections.Generic;

namespace ShoppingCartSvc.EventFeed
{
    public interface IEventStore
    {
        IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber);
        void Raise(string eventName, object content);
    }
}