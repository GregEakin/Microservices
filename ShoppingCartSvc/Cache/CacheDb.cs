// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCartSvc
// FILE:  Cache.cs
// AUTHOR:  Greg Eakin

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ShoppingCartSvc.Cache
{
    public class CacheDb : ICache
    {
        private readonly IDictionary<string, Tuple<DateTimeOffset, string>> _cache = new ConcurrentDictionary<string, Tuple<DateTimeOffset, string>>();

        public void Add(string key, TimeSpan ttl, string value)
        {
            var dateTimeOffset = DateTimeOffset.UtcNow.Add(ttl);
            _cache[key] = Tuple.Create(dateTimeOffset, value);
        }

        public string Get(string productsResource)
        {
            var tryGetValue = _cache.TryGetValue(productsResource, out var value);
            if (!tryGetValue) 
                return null;
            
            if (value.Item1 >= DateTimeOffset.UtcNow)
                return value.Item2;

            _cache.Remove(productsResource);
            return null;
        }
    }
}