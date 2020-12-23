using HelloMicroservices.EventFeed;
using HelloMicroservices.ShoppingCart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // GET: api/<ShoppingCartController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ShoppingCartController>/5
        [HttpGet("{id}")]
        public ActionResult<ShoppingCart.ShoppingCart> Get(int id)
        {
            if (id == 42)
            {
                var cart = new ShoppingCart.ShoppingCart(id);
                var item1 = new ShoppingCartItem(1, "Basic t-shirt", "a quiet t-shirt", new Money("eur", "40"));
                var item2 = new ShoppingCartItem(2, "Fancy shirt", "a loud t-shirt", new Money("eur", "50"));
                cart.AddItems(new[] { item1, item2 }, _eventStore);
                return cart;
            }
            else
            {
                var cart = _shoppingCartStore.Get(id);
                return cart;
            }
        }

        // POST api/<ShoppingCartController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            // var cart = _shoppingCartStore.Get(id);
        }

        // PUT api/<ShoppingCartController>/5
        [HttpPut("{id}/items")]
        public async Task Put(int id, [FromBody] string value)
        {
            var productCatalogIds = new int[0]; // this.Bind<int[]>();
            // var userId = (int)parameters.userid;

            var shoppingCart = _shoppingCartStore.Get(id);
            var shoppingCartItems = await _productCatalog.GetShoppingCartItems(productCatalogIds).ConfigureAwait(false);
            shoppingCart.AddItems(shoppingCartItems, _eventStore);
            _shoppingCartStore.Save(shoppingCart);
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("{id}/items")]
        public void Delete(int id)
        {
            var productCatalogIds = new int[0]; // this.Bind<int[]>();
            // var userId = (int)parameters.userid;

            var shoppingCart = _shoppingCartStore.Get(id);
            shoppingCart.RemoveItems(productCatalogIds, _eventStore);
            _shoppingCartStore.Save(shoppingCart);
        }
    }
}
