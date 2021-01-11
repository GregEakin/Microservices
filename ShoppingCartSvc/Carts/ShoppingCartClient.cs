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
// FILE:  ShoppingCartClient.cs
// AUTHOR:  Greg Eakin

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShoppingCartSvc.Carts
{
    public sealed class ShoppingCartClient : IDisposable
    {
        private readonly HttpClient _client = new();

        public void ShowProduct(ShoppingCart cart)
        {
            Console.WriteLine($"Id: {cart.Id}");
            foreach (var item in cart.Items)
            {
                Console.WriteLine("    id: {0}, name: {1}, desc: {2}, {3} {4}", item.ProductCatalogId, item.ProductName,
                    item.ProductDescription, item.Price.Currency, item.Price.Amount);
            }
        }

        public async Task<Uri> CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            var response = await _client.PostAsJsonAsync("api/ShoppingCart", shoppingCart);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        public async Task<ShoppingCart> GetShoppingCartAsync(string path)
        {
            var response = await _client.GetAsync(path);
            var shoppingCart = response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<ShoppingCart>()
                : null;
            return shoppingCart;
        }

        public async Task<ShoppingCart> UpdateProductAsync(ShoppingCart shoppingCart)
        {
            var response = await _client.PutAsJsonAsync($"api/ShoppingCart/{shoppingCart.Id}", shoppingCart);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            shoppingCart = await response.Content.ReadFromJsonAsync<ShoppingCart>();
            return shoppingCart;
        }

        public async Task<HttpStatusCode> DeleteShoppingCartAsync(string userId)
        {
            var response = await _client.DeleteAsync($"api/ShoppingCart/{userId}");
            return response.StatusCode;
        }

        public async Task RunAsync()
        {
            // Update port # in the following line.
            _client.BaseAddress = new Uri("http://localhost:64195/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                var shoppingCart = new ShoppingCart(42, Array.Empty<ShoppingCartItem>());

                var url = await CreateShoppingCartAsync(shoppingCart);
                Console.WriteLine($"Created at {url}");

                // Get the product
                shoppingCart = await GetShoppingCartAsync(url.PathAndQuery);
                ShowProduct(shoppingCart);

                // Update the product
                Console.WriteLine("Updating price...");
                // shoppingCart.Items.First().Price.Amount = "80";
                // await UpdateProductAsync(product);

                // Get the updated product
                shoppingCart = await GetShoppingCartAsync(url.PathAndQuery);
                ShowProduct(shoppingCart);

                // Delete the product
                var statusCode = await DeleteShoppingCartAsync("42");
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}