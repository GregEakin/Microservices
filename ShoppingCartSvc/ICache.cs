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