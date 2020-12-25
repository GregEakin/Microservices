// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCartTests
// FILE:  ShoppingCartTests.cs
// AUTHOR:  Greg Eakin

using System.Linq;
using ShoppingCartSvc.EventFeed;
using ShoppingCartSvc.Models;
using Moq;
using NUnit.Framework;

namespace ShoppingCartSvcTests
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

            cart.RemoveItems(new []{12}, _mockEventStore.Object);

            Assert.IsFalse(cart.Items.Any());
            _mockEventStore.VerifyAll();
        }
    }
}