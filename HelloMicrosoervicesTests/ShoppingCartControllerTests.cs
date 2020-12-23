// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicrosoervicesTests
// FILE:  ShoppingCartControllerTests.cs
// AUTHOR:  Greg Eakin

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloMicroservices;
using HelloMicroservices.Controllers;
using HelloMicroservices.EventFeed;
using HelloMicroservices.ShoppingCart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace HelloMicrosoervicesTests
{
    public class ShoppingCartControllerTests
    {
        [SetUp]
        public void Setup()
        {
            // services.AddSingleton<IShoppingCartStore, ShoppingCartStore>();
            // services.AddSingleton<IProductCatalogClient, ProductCatalogClient>();
            // services.AddSingleton<IEventStore, EventStore>();
        }

        [Test]
        public void GetCartTest()
        {
            var logger = new Mock<ILogger<ShoppingCartController>>();
            var shoppingCartStore = new Mock<IShoppingCartStore>();
            var productCatalog = new Mock<IProductCatalogClient>();
            var eventStore = new Mock<IEventStore>();

            var shoppingCart = new ShoppingCart(124);
            shoppingCartStore.Setup(t => t.Get(124)).Returns(shoppingCart);

            var request = new Mock<HttpRequest>();
            // request.Setup(x => x.Scheme).Returns("http");
            // request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            // request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));

            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);

            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            var controller = new ShoppingCartController(logger.Object, shoppingCartStore.Object, productCatalog.Object, eventStore.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Get(124);
            Assert.AreSame(shoppingCart, result.Value);
            shoppingCartStore.VerifyAll();
            request.VerifyAll();
        }

        [Test]
        public void AddCartItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", "Amount"));
            var items = Task.FromResult((IEnumerable<ShoppingCartItem>)new[] { item });

            var logger = new Mock<ILogger<ShoppingCartController>>();
            var shoppingCartStore = new Mock<IShoppingCartStore>();
            var productCatalog = new Mock<IProductCatalogClient>();
            productCatalog.Setup(t => t.GetShoppingCartItems(new[] { 12 })).Returns(items);
            var eventStore = new Mock<IEventStore>();

            var shoppingCart = new ShoppingCart(124);
            shoppingCartStore.Setup(t => t.Get(124)).Returns(shoppingCart);

            var request = new Mock<HttpRequest>();
            // request.Setup(x => x.Scheme).Returns("http");
            // request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            // request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));

            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);

            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            var controller = new ShoppingCartController(logger.Object, shoppingCartStore.Object, productCatalog.Object, eventStore.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = controller.Post(124, new[] { 12 });
            Assert.IsTrue(result.IsCompletedSuccessfully);
            shoppingCartStore.VerifyAll();
            productCatalog.VerifyAll();
            request.VerifyAll();
        }

        [Test]
        public void DeleteCartItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", "Amount"));
            var items = Task.FromResult((IEnumerable<ShoppingCartItem>)new[] { item });

            var logger = new Mock<ILogger<ShoppingCartController>>();
            var shoppingCartStore = new Mock<IShoppingCartStore>();
            var productCatalog = new Mock<IProductCatalogClient>();
            var eventStore = new Mock<IEventStore>();

            var shoppingCart = new ShoppingCart(124);
            shoppingCartStore.Setup(t => t.Get(124)).Returns(shoppingCart);
            shoppingCartStore.Setup(t => t.Save(It.IsAny<ShoppingCart>()));

            var request = new Mock<HttpRequest>();
            // request.Setup(x => x.Scheme).Returns("http");
            // request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            // request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));

            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);

            var controllerContext = new ControllerContext() { HttpContext = httpContext };

            var controller = new ShoppingCartController(logger.Object, shoppingCartStore.Object, productCatalog.Object, eventStore.Object)
            {
                ControllerContext = controllerContext,
            };

            controller.Delete(124, new[] { 12 });
            Assert.IsFalse(shoppingCart.Items.Any());
            shoppingCartStore.VerifyAll();
            request.VerifyAll();
        }
    }
}