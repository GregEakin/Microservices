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
// FILE:  ProductCatalogClientTests.cs
// AUTHOR:  Greg Eakin

using Moq;
using NUnit.Framework;
using ShoppingCartSvc.Cache;
using ShoppingCartSvc.Catalog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingCartSvcTests.Catalog
{
    public class ProductCatalogClientTests
    {
        private Mock<ICache> _mockCache;

        [SetUp]
        public void Setup()
        {
            _mockCache = new Mock<ICache>(MockBehavior.Strict);
        }

        [Test]
        public async Task GetItemsFromCatalogService_CacheHitTest()
        {
            var cachedResponse = "[{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}}]";
            _mockCache.Setup(t => t.Get("Products/products?id=0")).Returns(cachedResponse);

            var client = new ProductCatalogClient(_mockCache.Object);
            var response = await client.GetItemsFromCatalogService(new[] { 0 });
            var product = response.Single();
            Assert.That(0, Is.EqualTo(product.ProductCatalogId));
            Assert.That("foo0", Is.EqualTo(product.ProductName));
            Assert.That("bar", Is.EqualTo(product.ProductDescription));
            Assert.That(product.Price, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(product.Price.Currency), Is.False);
        }

        [Test]
        public async Task RequestProductFromProductCatalog_CacheHitTest()
        {
            var cachedResponse = "[{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}}]";
            _mockCache.Setup(t => t.Get("Products/products?id=0")).Returns(cachedResponse);

            var client = new ProductCatalogClient(_mockCache.Object);
            var response = await client.RequestProductFromProductCatalog(new[] { 0 });
            Assert.That(cachedResponse, Is.EqualTo(response));
        }

        // [Test]
        // public void ConvertToShoppingCartItems_BadResultTest() // get a null out of Deserialize 
        // {
        //     var client = new ProductCatalogClient(null);
        //     using var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        //     Assert.ThrowsAsync<HttpRequestException>(async () => await client.ConvertToShoppingCartItems(response));
        // }

        [Test]
        public void ConvertToShoppingCartItems_NoResultTest()
        {
            var client = new ProductCatalogClient(null);
            var result = client.ConvertToShoppingCartItems("[]");
            Assert.That(result.Any(), Is.False);
        }

        [Test]
        public void ConvertToShoppingCartItems_SingleTest()
        {
            var client = new ProductCatalogClient(null);
            var response = "[{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}}]";
            var result = client.ConvertToShoppingCartItems(response);
            var product = result.Single();
            Assert.That(0, Is.EqualTo(product.ProductCatalogId));
            Assert.That("foo0", Is.EqualTo(product.ProductName));
            Assert.That("bar", Is.EqualTo(product.ProductDescription));
            Assert.That(product.Price, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(product.Price.Currency), Is.False);
        }

        [Test]
        public void AddToCache_NoCacheControlTest()
        {
            var resource = "Products/products?id=0";
            var payload = "[{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}}]";

            var client = new ProductCatalogClient(_mockCache.Object);
            client.AddToCache(resource, null, payload);
        }

        [Test]
        public void AddToCache_CacheControlTest()
        {
            var resource = "Products/products?id=0";
            var maxAge = TimeSpan.FromHours(1);
            var payload = "[{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}}]";
            _mockCache.Setup(t => t.Add(resource, maxAge, payload));

            var client = new ProductCatalogClient(_mockCache.Object);
            client.AddToCache(resource, maxAge, payload);
        }
    }
}
