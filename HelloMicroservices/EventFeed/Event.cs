// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicroservices
// FILE:  Event.cs
// AUTHOR:  Greg Eakin

using System;

namespace HelloMicroservices.EventFeed
{
    public record Event(long SequenceNumber, DateTimeOffset OccurredAt, string Name, object Content);
}