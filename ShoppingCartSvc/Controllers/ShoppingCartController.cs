using ShoppingCartSvc.EventFeed;
using ShoppingCartSvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCartSvc.Controllers
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
            var shoppingCartItems = await _productCatalog.GetShoppingCartItems(productCatalogIds).ConfigureAwait(false);
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
