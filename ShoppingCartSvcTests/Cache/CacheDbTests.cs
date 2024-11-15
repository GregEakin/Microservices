﻿// Copyright 2020 Greg Eakin
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
            Assert.That(value, Is.Null);
        }

        [Test]
        public void CacheHitExpiredTest()
        {
            var cache = new CacheDb();
            var twoHoursAgo = TimeSpan.FromHours(-2.0);
            cache.Add("key", twoHoursAgo, "value");

            var value = cache.Get("key");
            Assert.That(value, Is.Null);
        }

        [Test]
        public void CacheHitTest()
        {
            var cache = new CacheDb();
            var fewMinutesAgo = TimeSpan.FromMinutes(30.0);
            cache.Add("key", fewMinutesAgo, "value");

            var value = cache.Get("key");
            Assert.That("value", Is.EqualTo(value));
        }
    }
}