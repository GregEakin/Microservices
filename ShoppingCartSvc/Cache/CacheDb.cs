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
        private static readonly IDictionary<string, Tuple<DateTimeOffset, string>> _cache = new ConcurrentDictionary<string, Tuple<DateTimeOffset, string>>();

        public void Add(string key, TimeSpan ttl, string value)
        {
            _cache[key] = Tuple.Create(DateTimeOffset.UtcNow.Add(ttl), value);
        }

        public object Get(string productsResource)
        {
            var tryGetValue = _cache.TryGetValue(productsResource, out var value);
            if (tryGetValue && value.Item1 > DateTimeOffset.UtcNow)
                return value;
            
            _cache.Remove(productsResource);
            return null;
        }
    }
}