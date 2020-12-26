// Copyright © 2020-2020. All Rights Reserved.
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

        [Test]
        public async Task RequestProductFromProductCatalog_CacheHitTest()
        {
            using var cachedResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "[{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}}]")
            };
            _mockCache.Setup(t => t.Get("Products/products?id=0")).Returns(cachedResponse);

            var client = new ProductCatalogClient(_mockCache.Object);
            var response = await client.RequestProductFromProductCatalog(new[] { 0 });
            Assert.AreSame(cachedResponse, response);
        }

        [Test]
        public void ConvertToShoppingCartItems_BadResultTest()
        {
            var client = new ProductCatalogClient(null);
            using var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            Assert.ThrowsAsync<HttpRequestException>(async () => await client.ConvertToShoppingCartItems(response));
        }

        [Test]
        public async Task ConvertToShoppingCartItems_NoResultTest()
        {
            var client = new ProductCatalogClient(null);
            using var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]")
            };
            var result = await client.ConvertToShoppingCartItems(response);
            Assert.IsFalse(result.Any());
        }

        [Test]
        public async Task ConvertToShoppingCartItems_SingleTest()
        {
            var client = new ProductCatalogClient(null);
            using var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "[{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}}]")
            };
            var result = await client.ConvertToShoppingCartItems(response);
            var product = result.Single();
            Assert.AreEqual(0, product.ProductCatalogId);
            Assert.AreEqual("foo0", product.ProductName);
            Assert.AreEqual("bar", product.ProductDescription);
            Assert.IsNotNull(product.Price);
            Assert.IsFalse(string.IsNullOrEmpty(product.Price.Currency));
        }
    }
}
