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
// SUBSYSTEM: ShoppingCartTests
// FILE:  ShoppingCartTests.cs
// AUTHOR:  Greg Eakin

using Moq;
using NUnit.Framework;
using ShoppingCartSvc.Carts;
using ShoppingCartSvc.Catalog;
using ShoppingCartSvc.EventFeed;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ShoppingCartSvcTests.Carts
{
    public class CartTests
    {
        private Mock<IEventStore> _mockEventStore;

        [SetUp]
        public void Setup()
        {
            // var mockShoppingCartStore = new Mock<IShoppingCartStore>();
            // services.AddSingleton<IProductCatalogClient, ProductCatalogClient>();
            _mockEventStore = new Mock<IEventStore>(MockBehavior.Strict);
        }

        [Test]
        public void AddItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", 123.456m));
            var items = new[] { item };

            // _mockEventStore.Setup(t => t.Raise("ShoppingCartItemAdded", new { UserId = 234, item = item })).Returns(0uL);
            _mockEventStore.Setup(t => t.Raise("ShoppingCartItemAdded", It.IsAny<object>())).Returns(0uL);

            var cart = new ShoppingCart(234, new ShoppingCartItem[0]);
            cart.AddItems(items, _mockEventStore.Object);

            Assert.AreEqual(items, cart.Items);
        }

        [Test]
        public void RemoveItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", 123.45m));
            var items = new[] { item };
            var cart = new ShoppingCart(234, items);
            _mockEventStore.Setup(t => t.Raise("ShoppingCartItemRemoved", It.IsAny<object>())).Returns(0uL);

            cart.RemoveItems(new[] { 12 }, _mockEventStore.Object);

            Assert.IsFalse(cart.Items.Any());
        }

        [Test]
        public void Deserialize3_ProductCatalogProductTest()
        {
            var msg = "[{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}},"
                      + "{\"productId\":\"2\",\"productName\":\"foo2\",\"productDescription\":\"bar\",\"price\":{}},"
                      + "{\"productId\":\"3\",\"productName\":\"foo3\",\"productDescription\":\"bar\",\"price\":{}}]";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var products = JsonSerializer.Deserialize<List<ProductCatalogProduct>>(msg, options);

            Assert.IsNotNull(products);
            Assert.AreEqual(3, products.Count);
            Assert.AreEqual("0", products[0].ProductId);
            Assert.AreEqual("2", products[1].ProductId);
            Assert.AreEqual("3", products[2].ProductId);
        }

        [Test]
        public void Deserialize_ProductCatalogProductTest()
        {
            var msg = "{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var product = JsonSerializer.Deserialize<ProductCatalogProduct>(msg, options);

            Assert.IsNotNull(product);
            Assert.AreEqual("0", product.ProductId);
            Assert.AreEqual("foo0", product.ProductName);
            Assert.AreEqual("bar", product.ProductDescription);
            Assert.IsNotNull(product.Price);
            Assert.IsTrue(string.IsNullOrEmpty(product.Price.Currency));
        }

        [Test]
        public void DeserializeShort_ProductCatalogProductTest()
        {
            var msg = "{\"productId\":\"0\"}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var product = JsonSerializer.Deserialize<ProductCatalogProduct>(msg, options);

            Assert.IsNotNull(product);
            Assert.AreEqual("0", product.ProductId);
            Assert.IsTrue(string.IsNullOrEmpty(product.ProductName));
            Assert.IsTrue(string.IsNullOrEmpty(product.ProductDescription));
            Assert.IsNull(product.Price);
        }
    }
}