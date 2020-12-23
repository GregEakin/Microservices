// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicroservices
// FILE:  ShopingCartClient.cs
// AUTHOR:  Greg Eakin

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HelloMicroservices.ShoppingCart;

namespace HelloMicroservices
{
    public class ShoppingCartClient
    {
        private readonly HttpClient _client = new();

        public void ShowProduct(ShoppingCart.ShoppingCart cart)
        {
            Console.WriteLine($"UserId: {cart.UserId}");
            foreach (var item in cart.Items)
            {
                Console.WriteLine("    id: {0}, name: {1}, desc: {2}, {3} {4}", item.ProductCatalogId, item.ProductName,
                    item.Description, item.Price.Currency, item.Price.Amount);
            }
        }

        public async Task<Uri> CreateShoppingCartAsync(ShoppingCart.ShoppingCart shoppingCart)
        {
            var response = await _client.PostAsJsonAsync("api/ShoppingCart", shoppingCart);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        public async Task<ShoppingCart.ShoppingCart> GetShoppingCartAsync(string path)
        {
            var response = await _client.GetAsync(path);
            var shoppingCart = response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<ShoppingCart.ShoppingCart>()
                : null;
            return shoppingCart;
        }

        public async Task<ShoppingCart.ShoppingCart> UpdateProductAsync(ShoppingCart.ShoppingCart shoppingCart)
        {
            var response = await _client.PutAsJsonAsync($"api/ShoppingCart/{shoppingCart.UserId}", shoppingCart);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            shoppingCart = await response.Content.ReadFromJsonAsync<ShoppingCart.ShoppingCart>();
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
                var shoppingCart = new ShoppingCart.ShoppingCart(42);

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
    }
}