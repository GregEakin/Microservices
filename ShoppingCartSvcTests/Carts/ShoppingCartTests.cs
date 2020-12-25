// Copyright © 2020-2020. All Rights Reserved.
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
            _mockEventStore = new Mock<IEventStore>();
        }

        [Test]
        public void AddItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", 123));
            var items = new[] { item };

            // mockEventStore.Setup(t =>
            //     t.Raise("ShoppingCartItemAdded", new { UserId = 234, item = item })
            // );

            _mockEventStore.Setup(t => t.Raise("ShoppingCartItemAdded", It.IsAny<object>()));

            var cart = new ShoppingCart(234, new ShoppingCartItem[0]);
            cart.AddItems(items, _mockEventStore.Object);

            Assert.AreEqual(items, cart.Items);
            _mockEventStore.VerifyAll();
        }

        [Test]
        public void RemoveItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", 123));
            var items = new[] { item };
            var cart = new ShoppingCart(234, items);
            _mockEventStore.Setup(t => t.Raise("ShoppingCartItemRemoved", It.IsAny<object>()));

            cart.RemoveItems(new[] { 12 }, _mockEventStore.Object);

            Assert.IsFalse(cart.Items.Any());
            _mockEventStore.VerifyAll();
        }

        [Test]
        public void Test1()
        {
            var msg = "[{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}},"
                      + "{\"productId\":\"2\",\"productName\":\"foo2\",\"productDescription\":\"bar\",\"price\":{}},"
                      + "{\"productId\":\"3\",\"productName\":\"foo3\",\"productDescription\":\"bar\",\"price\":{}}]";
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var products = JsonSerializer.Deserialize<List<ProductCatalogProduct>>(msg, options);

            Assert.IsNotNull(products);
            Assert.AreEqual(3, products.Count);
            Assert.AreEqual("0", products[0].ProductId);
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            var msg = "{\"productId\":\"0\",\"productName\":\"foo0\",\"productDescription\":\"bar\",\"price\":{}}";
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var product = JsonSerializer.Deserialize<ProductCatalogProduct>(msg, options);

            Assert.IsNotNull(product);
            Assert.AreEqual("0", product.ProductId);
            Assert.Pass();
        }

        [Test]
        public void Test3()
        {
            var msg = "{\"productId\":\"0\"}";
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var product = JsonSerializer.Deserialize<ProductCatalogProduct>(msg, options);

            Assert.IsNotNull(product);
            Assert.AreEqual("0", product.ProductId);
            Assert.Pass();
        }
    }
}