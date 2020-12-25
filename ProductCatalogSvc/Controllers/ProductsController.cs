using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductCatalogSvc.Products;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductCatalogSvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductStore _productStore;

        public ProductsController(ILogger<ProductsController> logger, IProductStore productProductStore)
        {
            _logger = logger;
            _productStore = productProductStore;
        }

        // GET api/<ProductsController>/products
        [HttpGet("items")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 86400)]
        public IEnumerable<ProductCatalogProduct> Get([FromQuery] int[] id)
        {
            var products = _productStore.GetProductsByIds(id);
            return products;
        }
    }
}
