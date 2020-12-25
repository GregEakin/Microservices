// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  ProductCatalogProduct.cs
// AUTHOR:  Greg Eakin

using ShoppingCartSvc.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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


        // http://192.168.40.140:8086/api/Products/items?id=2&id=3
        private const string productCatalogBaseUrl = @"http://microservices_productsvc_1/api/";
        private const string getProductPathTemplate = "Products/products?id={0}";

        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds) =>
            // exponentialRetryPolicy.ExecuteAsync(async () => await GetItemsFromCatalogService(productCatalogIds).ConfigureAwait(false));
            await GetItemsFromCatalogService(productCatalogIds).ConfigureAwait(false);

        private async Task<IEnumerable<ShoppingCartItem>> GetItemsFromCatalogService(int[] productCatalogIds)
        {
            var response = await RequestProductFromProductCatalog(productCatalogIds).ConfigureAwait(false);
            return await ConvertToShoppingCartItems(response).ConfigureAwait(false);
        }

        private static async Task<HttpResponseMessage> RequestProductFromProductCatalog(IEnumerable<int> productCatalogIds)
        {
            var productsResource = string.Format(getProductPathTemplate, string.Join("&id=", productCatalogIds));
            using var httpClient = new HttpClient { BaseAddress = new Uri(productCatalogBaseUrl) };
            return await httpClient.GetAsync(productsResource).ConfigureAwait(false);
        }

        private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var readAsStringAsync = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            var products = JsonSerializer.Deserialize<List<ProductCatalogProduct>>(readAsStringAsync, options);
            return products.Select(p => new ShoppingCartItem(
                  int.Parse(p.ProductId),
                  p.ProductName,
                  p.ProductDescription,
                  p.Price
              ));
        }

        public record ProductCatalogProduct3(string ProductId, string ProductName, string ProductDescription, Money Price);
    }

    public class ProductCatalogProduct
    {
        public ProductCatalogProduct(string productId, string productName, string productDescription, Money price)
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            Price = price;
        }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public Money Price { get; set; }
    }
}
