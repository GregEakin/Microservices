// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCartSvcTests
// FILE:  CacheDbTests.cs
// AUTHOR:  Greg Eakin

using System;
using NUnit.Framework;
using ShoppingCartSvc.Cache;

namespace ShoppingCartSvcTests.Cache
{
    public class CacheDbTests
    {
        [Test]
        public void CacheMissTest()
        {
            var cache = new CacheDb();
            var value = cache.Get("key");
            Assert.IsNull(value);
        }

        [Test]
        public void CacheHitExpiredTest()
        {
            var cache = new CacheDb();
            var twoHoursAgo = TimeSpan.FromHours(-2.0);
            cache.Add("key", twoHoursAgo, "value");

            var value = cache.Get("key");
            Assert.IsNull(value);
        }

        [Test]
        public void CacheHitTest()
        {
            var cache = new CacheDb();
            var fewMinutesAgo = TimeSpan.FromMinutes(30.0);
            cache.Add("key", fewMinutesAgo, "value");

            var value = cache.Get("key");
            Assert.AreEqual("value", value);
        }
    }
}