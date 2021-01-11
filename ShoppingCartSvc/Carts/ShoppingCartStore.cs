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
// FILE:  ShoppingCartStore.cs
// AUTHOR:  Greg Eakin

using System;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartSvc.Carts
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private const string ConnectionString =
            @"Host=microservices_cartdb_1;Port=5432;Database=cartapp;Username=cartapp;Password=cartpw";

        private const string ReadItemsSql =
            @"select ""ShoppingCart"".id, ""ProductCatalogId"", ""ProductName"", ""ProductDescription"", ""Currency"", ""Amount""
from ""ShoppingCart"" left join ""ShoppingCartItems"" on ""ShoppingCart"".""id"" = ""ShoppingCartItems"".""ShoppingCartId""
where ""ShoppingCart"".""UserId"" = @UserId";

        public async Task<ShoppingCart> Get(int userId)
        {
            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            ShoppingCart shoppingCart = null;
            var items = await conn.QueryAsync<int, ShoppingCartItem, Money, ShoppingCart>(ReadItemsSql,
                (id, shoppingCartItem, money) =>
                {
                    // Console.WriteLine("Read DB: {0}, {1}, {2}, {3}, {4}, {5}", id, shoppingCartItem?.ProductCatalogId,
                    //     shoppingCartItem?.ProductName, shoppingCartItem?.ProductDescription, 
                    //     money?.Amount, money?.Currency);

                    shoppingCart ??= new ShoppingCart(id, Array.Empty<ShoppingCartItem>());
                    if (shoppingCartItem == null)
                        return shoppingCart;

                    shoppingCart.AddItems(new[] { shoppingCartItem }, null);

                    if (money == null)
                    {
                        // Console.WriteLine("Money is null!");
                        shoppingCartItem.Price ??= new Money("none", 9m);
                        return shoppingCart;
                    }

                    shoppingCartItem.Price ??= new Money();
                    shoppingCartItem.Price.Amount = money.Amount;
                    shoppingCartItem.Price.Currency = money.Currency;
                    return shoppingCart;
                },
                new { UserId = userId },
                splitOn: "ProductCatalogId,Currency");
            return items.FirstOrDefault();
        }

        private const string DeleteAllForShoppingCartSql = @"DELETE FROM ""ShoppingCartItems"" WHERE ""ShoppingCartId"" = @Id";

        private const string AddAllForShoppingCartSql =
            @"insert into ""ShoppingCartItems"" (""ShoppingCartId"", ""ProductCatalogId"", ""ProductName"", ""ProductDescription"", ""Amount"", ""Currency"")
values 
(@ShoppingCartId, @ProductCatalogId, @ProductName, @ProductDescription, @Amount, @Currency)";

        public async Task Save(ShoppingCart shoppingCart)
        {
            await using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();
            await using var tx = await conn.BeginTransactionAsync();
            await conn.ExecuteAsync(DeleteAllForShoppingCartSql, new { shoppingCart.Id }, tx)
                .ConfigureAwait(false);

            foreach (var item in shoppingCart.Items)
            {
                item.Price ??= new Money("none", 1m);
                // Console.WriteLine("Item: {0}, {1}, {2}, {3}, {4}, {5}", shoppingCart.Id, item.ProductCatalogId,
                //     item.ProductName, item.ProductDescription, item.Price.Amount, item.Price.Currency);
            }

            var items = shoppingCart.Items.Select(item => new
            {
                shoppingCartId = shoppingCart.Id,
                item.ProductCatalogId,
                item.ProductName,
                item.ProductDescription,
                item.Price.Amount,
                item.Price.Currency
            });

            await conn.ExecuteAsync(AddAllForShoppingCartSql, items, tx)
                .ConfigureAwait(false);

            await tx.CommitAsync();
        }
    }
}
