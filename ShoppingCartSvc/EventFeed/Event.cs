// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  Event.cs
// AUTHOR:  Greg Eakin

using System;

namespace ShoppingCartSvc.EventFeed
{
    public record Event(long SequenceNumber, DateTime OccurredAt, string Name, object Content);
}