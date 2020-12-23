// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicroservices
// FILE:  ProductCatalogProduct.cs
// AUTHOR:  Greg Eakin

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HelloMicroservices.Models;
using System.Text.Json;

namespace HelloMicroservices
{
    public class ProductCatalogClient : IProductCatalogClient
    {
        // private static Policy exponentialRetryPolicy = Policy.Handle<Exception>()
        //     .WaitAndRetryAsync(
        //       3,
        //       attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)), (ex, _) => Console.WriteLine(ex.ToString()));

        private const string productCatalogBaseUrl = @"http://private-05cc8-chapter2productcataloguemicroservice.apiary-mock.com";
        private const string getProductPathTemplate = "/products?productIds=[{0}]";

        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds) =>
            // exponentialRetryPolicy.ExecuteAsync(async () => await GetItemsFromCatalogService(productCatalogIds).ConfigureAwait(false));
            await GetItemsFromCatalogService(productCatalogIds).ConfigureAwait(false);

        private async Task<IEnumerable<ShoppingCartItem>> GetItemsFromCatalogService(int[] productCatalogIds)
        {
            var response = await RequestProductFromProductCatalog(productCatalogIds).ConfigureAwait(false);
            return await ConvertToShoppingCartItems(response).ConfigureAwait(false);
        }

        private static async Task<HttpResponseMessage> RequestProductFromProductCatalog(int[] productCatalogIds)
        {
            var productsResource = string.Format(getProductPathTemplate, string.Join(",", productCatalogIds));
            using var httpClient = new HttpClient { BaseAddress = new Uri(productCatalogBaseUrl) };
            return await httpClient.GetAsync(productsResource).ConfigureAwait(false);
        }

        private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var products = JsonSerializer.Deserialize<List<ProductCatalogProduct>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return products.Select(p => new ShoppingCartItem(
                  int.Parse(p.ProductId),
                  p.ProductName,
                  p.ProductDescription,
                  p.Price
              ));
        }

        private record ProductCatalogProduct(string ProductId, string ProductName, string ProductDescription, Money Price);
    }
}