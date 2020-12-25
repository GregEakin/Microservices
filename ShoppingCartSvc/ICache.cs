// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  ICache.cs
// AUTHOR:  Greg Eakin

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ShoppingCartSvc
{
    public interface ICache
    {
        void Add(string key, object value, TimeSpan ttl);
        object Get(string productsResource);
    }

    public class Cache : ICache
    {
        private static readonly IDictionary<string, Tuple<DateTimeOffset, object>> cache = new ConcurrentDictionary<string, Tuple<DateTimeOffset, object>>();

        public void Add(string key, object value, TimeSpan ttl)
        {
            cache[key] = Tuple.Create(DateTimeOffset.UtcNow.Add(ttl), value);
        }

        public object Get(string productsResource)
        {
            var tryGetValue = cache.TryGetValue(productsResource, out var value);
            if (tryGetValue && value.Item1 > DateTimeOffset.UtcNow)
                return value;
            
            cache.Remove(productsResource);
            return null;
        }
    }
}