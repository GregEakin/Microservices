// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicrosoervicesTests
// FILE:  ShoppingCartTests.cs
// AUTHOR:  Greg Eakin

using System.Linq;
using HelloMicroservices.EventFeed;
using HelloMicroservices.Models;
using Moq;
using NUnit.Framework;

namespace HelloMicrosoervicesTests
{
    public class ShoppingCartTests
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
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", "Amount"));
            var items = new[] { item };

            // mockEventStore.Setup(t =>
            //     t.Raise("ShoppingCartItemAdded", new { UserId = 234, item = item })
            // );

            _mockEventStore.Setup(t => t.Raise("ShoppingCartItemAdded", It.IsAny<object>()));

            var cart = new ShoppingCart(234);
            cart.AddItems(items, _mockEventStore.Object);

            Assert.AreEqual(items, cart.Items);
            _mockEventStore.VerifyAll();
        }

        [Test]
        public void RemoveItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", "Amount"));
            var items = new[] { item };
            var cart = new ShoppingCart(234, items);
            _mockEventStore.Setup(t => t.Raise("ShoppingCartItemRemoved", It.IsAny<object>()));

            cart.RemoveItems(new []{12}, _mockEventStore.Object);

            Assert.IsFalse(cart.Items.Any());
            _mockEventStore.VerifyAll();
        }
    }
}