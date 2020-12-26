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
// FILE:  ProductCatalogProduct.cs
// AUTHOR:  Greg Eakin

using ShoppingCartSvc.Cache;
using ShoppingCartSvc.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingCartSvc.Catalog
{
    public class ProductCatalogClient : IProductCatalogClient
    {
        // private static Policy exponentialRetryPolicy = Policy.Handle<Exception>()
        //     .WaitAndRetryAsync(
        //       3,
        //       attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)), (ex, _) => Console.WriteLine(ex.ToString()));

        // http://microservices_productsvc_1/api/Products/items?id=2&id=3
        private const string productCatalogBaseUrl = @"http://microservices_productsvc_1/api/";
        private const string getProductPathTemplate = "Products/products?id={0}";

        private readonly ICache _cache;

        public ProductCatalogClient(ICache cache)
        {
            _cache = cache;
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds) =>
            // exponentialRetryPolicy.ExecuteAsync(async () => await GetItemsFromCatalogService(productCatalogIds).ConfigureAwait(false));
            await GetItemsFromCatalogService(productCatalogIds).ConfigureAwait(false);

        public async Task<IEnumerable<ShoppingCartItem>> GetItemsFromCatalogService(int[] productCatalogIds)
        {
            var response = await RequestProductFromProductCatalog(productCatalogIds).ConfigureAwait(false);
            return await ConvertToShoppingCartItems(response).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> RequestProductFromProductCatalog(IEnumerable<int> productCatalogIds)
        {
            var productsResource = string.Format(getProductPathTemplate, string.Join("&id=", productCatalogIds));
            if (_cache.Get(productsResource) is HttpResponseMessage response)
                return response;

            using var httpClient = new HttpClient { BaseAddress = new Uri(productCatalogBaseUrl) };
            var response1 = await httpClient.GetAsync(productsResource).ConfigureAwait(false);
            var maxAge = response1.Headers.CacheControl?.MaxAge;
            var payload = await response1.Content.ReadAsStringAsync().ConfigureAwait(false);
            AddToCache(productsResource, maxAge, payload);
            return response1;
        }

        public async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var readAsStringAsync = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var products = JsonSerializer.Deserialize<List<ProductCatalogProduct>>(readAsStringAsync, options);
            if (products == null)
                throw new NullReferenceException("JsonSerializer.Deserialize() returned null.");

            return products.Select(p => new ShoppingCartItem(
                  int.Parse(p.ProductId),
                  p.ProductName,
                  p.ProductDescription,
                  p.Price
              ));
        }

        public void AddToCache(string resource, TimeSpan? maxAge, string response)
        {
            if (!maxAge.HasValue)
                return;

            _cache.Add(resource, maxAge.Value, response);
        }
    }
}
