using System;
using System.Threading.Tasks;
using HelloMicroservices.EventFeed;
using HelloMicroservices.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloMicroservices.Controllers
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
        [HttpGet("{userId}")]
        public ActionResult<ShoppingCart.ShoppingCart> Get(int userId)
        {
            var cart = _shoppingCartStore.Get(userId);
            return cart;
        }

        // POST api/<ShoppingCart>/5
        [HttpPost("{userId}")]
        public async Task Post(int userId, [FromBody] int[] productCatalogIds)
        {
            if (productCatalogIds == null)
                await Task.FromException(new NullReferenceException(nameof(productCatalogIds)));

            var shoppingCart = _shoppingCartStore.Get(userId);
            var shoppingCartItems = await _productCatalog.GetShoppingCartItems(productCatalogIds).ConfigureAwait(false);
            shoppingCart.AddItems(shoppingCartItems, _eventStore);
            _shoppingCartStore.Save(shoppingCart);
        }

        // DELETE api/<ShoppingCart>/5
        [HttpDelete("{userId}")]
        public void Delete(int userId, [FromBody] int[] productCatalogIds)
        {
            var shoppingCart = _shoppingCartStore.Get(userId);
            shoppingCart.RemoveItems(productCatalogIds, _eventStore);
            _shoppingCartStore.Save(shoppingCart);
        }
    }
}
