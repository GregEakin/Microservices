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
// FILE:  ShoppingCartControllerTests.cs
// AUTHOR:  Greg Eakin

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ShoppingCartSvc.Carts;
using ShoppingCartSvc.Catalog;
using ShoppingCartSvc.Controllers;
using ShoppingCartSvc.EventFeed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartSvcTests.Carts
{
    public class CartControllerTests
    {
        [SetUp]
        public void Setup()
        {
            // services.AddSingleton<IShoppingCartStore, ShoppingCartStore>();
            // services.AddSingleton<IProductCatalogClient, ProductCatalogClient>();
            // services.AddSingleton<IEventStore, EventStore>();
        }

        [Test]
        public async Task GetCartTest()
        {
            var logger = new Mock<ILogger<ShoppingCartController>>();
            var shoppingCartStore = new Mock<IShoppingCartStore>();
            var productCatalog = new Mock<IProductCatalogClient>();
            var eventStore = new Mock<IEventStore>();

            var shoppingCart = new ShoppingCart(124, new ShoppingCartItem[0]);
            shoppingCartStore.Setup(t => t.Get(124)).Returns(Task.FromResult(shoppingCart));

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

            var result = await controller.Get(124);
            Assert.AreSame(shoppingCart, result.Value);
            shoppingCartStore.VerifyAll();
            request.VerifyAll();
        }

        [Test]
        public async Task AddCartItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", 4532.15m));
            var items = Task.FromResult((IEnumerable<ShoppingCartItem>)new[] { item });

            var logger = new Mock<ILogger<ShoppingCartController>>();
            var shoppingCartStore = new Mock<IShoppingCartStore>();
            var productCatalog = new Mock<IProductCatalogClient>();
            productCatalog.Setup(t => t.GetShoppingCartItems(new[] { 12 })).Returns(items);
            var eventStore = new Mock<IEventStore>();

            var shoppingCart = new ShoppingCart(124, new ShoppingCartItem[0]);
            shoppingCartStore.Setup(t => t.Get(124)).Returns(Task.FromResult(shoppingCart));

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

            var response = await controller.Post(124, new[] { 12 });
            Assert.AreEqual(200, (response as StatusCodeResult)?.StatusCode);
            shoppingCartStore.VerifyAll();
            productCatalog.VerifyAll();
            request.VerifyAll();
        }

        [Test]
        public async Task DeleteCartItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", 987.12m));
            var items = Task.FromResult((IEnumerable<ShoppingCartItem>)new[] { item });

            var logger = new Mock<ILogger<ShoppingCartController>>();
            var shoppingCartStore = new Mock<IShoppingCartStore>();
            var productCatalog = new Mock<IProductCatalogClient>();
            var eventStore = new Mock<IEventStore>();

            var shoppingCart = new ShoppingCart(124, new ShoppingCartItem[0]);
            shoppingCartStore.Setup(t => t.Get(124)).Returns(Task.FromResult(shoppingCart));
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

            await controller.Delete(124, new[] { 12 });
            Assert.IsFalse(shoppingCart.Items.Any());
            shoppingCartStore.VerifyAll();
            request.VerifyAll();
        }
    }
}