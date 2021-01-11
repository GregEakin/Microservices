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
using ShoppingCartSvc.EventFeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartSvcTests.Carts
{
    public class CartControllerTests
    {
        private Mock<ILogger<ShoppingCartController>> _logger;
        private Mock<IShoppingCartStore> _shoppingCartStore;
        private Mock<IProductCatalogClient> _productCatalog;
        private Mock<IEventStore> _eventStore;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ShoppingCartController>>(MockBehavior.Strict);
            _shoppingCartStore = new Mock<IShoppingCartStore>(MockBehavior.Strict);
            _productCatalog = new Mock<IProductCatalogClient>(MockBehavior.Strict);
            _eventStore = new Mock<IEventStore>(MockBehavior.Strict);
        }

        [Test]
        public async Task GetCartTest()
        {
            var shoppingCart = new ShoppingCart(124, Array.Empty<ShoppingCartItem>());
            _shoppingCartStore.Setup(t => t.Get(124)).Returns(Task.FromResult(shoppingCart));

            var request = new Mock<HttpRequest>();
            // request.Setup(x => x.Scheme).Returns("http");
            // request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            // request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));
            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);
            var controllerContext = new ControllerContext { HttpContext = httpContext };
            var controller = new ShoppingCartController(_logger.Object, _shoppingCartStore.Object, _productCatalog.Object, _eventStore.Object)
            {
                ControllerContext = controllerContext,
            };

            var result = await controller.Get(124);
            Assert.AreSame(shoppingCart, result.Value);
        }

        [Test]
        public async Task AddCartItemTest()
        {
            ShoppingCartItem item = new(12, "ProductName", "Description", new Money("Currency", 4532.15m));
            var items = Task.FromResult((IEnumerable<ShoppingCartItem>)new[] { item });
            _productCatalog.Setup(t => t.GetShoppingCartItems(new[] { 12 })).Returns(items);

            //_eventStore.Setup(t => t.Raise("ShoppingCartItemAdded", new { Id = 124, item })).Returns(0uL);
            _eventStore.Setup(t => t.Raise("ShoppingCartItemAdded", It.IsAny<object>())).Returns(0uL);

            var shoppingCart = new ShoppingCart(124, Array.Empty<ShoppingCartItem>());
            _shoppingCartStore.Setup(t => t.Get(124)).Returns(Task.FromResult(shoppingCart));
            _shoppingCartStore.Setup(t => t.Save(It.IsAny<ShoppingCart>())).Returns(Task.CompletedTask);

            var request = new Mock<HttpRequest>();
            // request.Setup(x => x.Scheme).Returns("http");
            // request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            // request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));
            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);
            var controllerContext = new ControllerContext { HttpContext = httpContext };
            var controller = new ShoppingCartController(_logger.Object, _shoppingCartStore.Object, _productCatalog.Object, _eventStore.Object)
            {
                ControllerContext = controllerContext,
            };

            var response = await controller.Post(124, new[] { 12 });
            Assert.AreEqual(200, (response as StatusCodeResult)?.StatusCode);
        }

        [Test]
        public async Task DeleteCartItemTest()
        {
            //_eventStore.Setup(t => t.Raise("ShoppingCartItemRemoved", new { Id = 124, item = 12})).Returns(0uL);
            _eventStore.Setup(t => t.Raise("ShoppingCartItemRemoved", It.IsAny<object>())).Returns(0uL);

            var item = new ShoppingCartItem(12, "ProductName", "Description", new Money("USD", 987.12m));
            var shoppingCart = new ShoppingCart(124, new[] { item });
            _shoppingCartStore.Setup(t => t.Get(124)).Returns(Task.FromResult(shoppingCart));
            _shoppingCartStore.Setup(t => t.Save(It.IsAny<ShoppingCart>())).Returns(Task.CompletedTask);

            var request = new Mock<HttpRequest>();
            // request.Setup(x => x.Scheme).Returns("http");
            // request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:8080"));
            // request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));
            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);
            var controllerContext = new ControllerContext { HttpContext = httpContext };
            var controller = new ShoppingCartController(_logger.Object, _shoppingCartStore.Object, _productCatalog.Object, _eventStore.Object)
            {
                ControllerContext = controllerContext,
            };

            await controller.Delete(124, new[] { 12 });
            Assert.IsFalse(shoppingCart.Items.Any());
        }
    }
}