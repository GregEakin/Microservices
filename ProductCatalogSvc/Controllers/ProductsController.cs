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
// SUBSYSTEM: ProductCatalogSvc
// FILE:  ProductsController.cs
// AUTHOR:  Greg Eakin

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
        [HttpGet("products")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 86400)]
        public IEnumerable<ProductCatalogProduct> Get([FromQuery] int[] id)
        {
            var products = _productStore.GetProductsByIds(id);
            return products;
        }
    }
}
