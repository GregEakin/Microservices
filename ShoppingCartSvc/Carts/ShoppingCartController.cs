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
// SUBSYSTEM: ShoppingCart
// FILE:  ShoppingCartController.cs
// AUTHOR:  Greg Eakin

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCartSvc.Catalog;
using ShoppingCartSvc.EventFeed;
using System.Threading.Tasks;

namespace ShoppingCartSvc.Carts
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IShoppingCartStore _shoppingCartStore;
        private readonly IProductCatalogClient _productCatalog;
        private readonly IEventStore _eventStore;

        public ShoppingCartController(ILogger<ShoppingCartController> logger, IShoppingCartStore shoppingCartStore, IProductCatalogClient productCatalog, IEventStore eventStore)
        {
            _logger = logger;
            _shoppingCartStore = shoppingCartStore;
            _productCatalog = productCatalog;
            _eventStore = eventStore;
        }

        // GET api/<ShoppingCart>/5
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<ShoppingCart>> Get(int userId)
        {
            var cart = await _shoppingCartStore.Get(userId);
            return cart;
        }

        // POST api/<ShoppingCart>/5
        [HttpPost("{userId:int}/items")]
        public async Task<ActionResult> Post(int userId, [FromBody] int[] productCatalogIds)
        {
            if (productCatalogIds == null)
                // await Task.FromException(new NullReferenceException(nameof(productCatalogIds)));
                // throw new HttpResponseException(msg);
                return BadRequest();

            var shoppingCart = await _shoppingCartStore.Get(userId);
            var shoppingCartItems = await _productCatalog.GetShoppingCartItems(productCatalogIds)
                .ConfigureAwait(false);
            shoppingCart.AddItems(shoppingCartItems, _eventStore);
            await _shoppingCartStore.Save(shoppingCart);
            return Ok();
        }

        // DELETE api/<ShoppingCart>/5
        [HttpDelete("{userId:int}/items")]
        public async Task Delete(int userId, [FromBody] int[] productCatalogIds)
        {
            var shoppingCart = await _shoppingCartStore.Get(userId);
            shoppingCart.RemoveItems(productCatalogIds, _eventStore);
            await _shoppingCartStore.Save(shoppingCart);
        }
    }
}
