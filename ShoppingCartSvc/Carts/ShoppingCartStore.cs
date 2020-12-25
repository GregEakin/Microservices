// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  ShoppingCartStore.cs
// AUTHOR:  Greg Eakin

using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Npgsql;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ShoppingCartSvc.Carts
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private const string connectionString =
            @"Host=microservices_cartdb_1;Port=5432;Database=cartapp;Username=cartapp;Password=cartpw";

        private const string readItemsSql =
            @"select ""ShoppingCartId"", ""ProductCatalogId"", ""ProductName"", ""ProductDescription"", ""Currency"", ""Amount""
from ""public"".""ShoppingCart"", ""public"".""ShoppingCartItems""
where ""ShoppingCart"".""id"" = ""ShoppingCartItems"".""ShoppingCartId""
and ""ShoppingCart"".""UserId"" = @UserId";

        public async Task<ShoppingCart> Get(int userId)
        {
            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            var id = -1;
            var items = await conn.QueryAsync<int, ShoppingCartItem, Money, ShoppingCartItem>(readItemsSql,
                (cartId, shoppingCartItem, money) =>
                {
                    id = cartId;
                    if (money == null)
                    {
                        Console.WriteLine("Money is null!");
                        shoppingCartItem.Price ??= new Money("none", 9);
                        return shoppingCartItem;
                    }

                    shoppingCartItem.Price ??= new Money();
                    shoppingCartItem.Price.Amount = money.Amount;
                    shoppingCartItem.Price.Currency = money.Currency;
                    return shoppingCartItem;
                },
                new { UserId = userId },
                splitOn: "ProductCatalogId,Currency");
            return new ShoppingCart(id, items);
        }

        // DELETE FROM table_name WHERE condition RETURNING(select_list | *)
        // DELETE FROM t1 USING t2 WHERE t1.id = t2.id

        private const string deleteAllForShoppingCartSql = @"DELETE FROM ""ShoppingCartItems"" WHERE ""ShoppingCartId"" = @Id";

        private const string addAllForShoppingCartSql =
            @"insert into ""ShoppingCartItems"" (""ShoppingCartId"", ""ProductCatalogId"", ""ProductName"", ""ProductDescription"", ""Amount"", ""Currency"")
values 
(@ShoppingCartId, @ProductCatalogId, @ProductName, @ProductDescription, @Amount, @Currency)";

        public async Task Save(ShoppingCart shoppingCart)
        {
            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            await using var tx = conn.BeginTransaction();
            await conn.ExecuteAsync(
                deleteAllForShoppingCartSql,
                new { Id = shoppingCart.Id },
                tx).ConfigureAwait(false);

            foreach (var item in shoppingCart.Items)
            {
                item.Price ??= new Money("none", 1);
                Console.WriteLine("Item: {0}, {1}, {2}, {3}, {4}, {5}", shoppingCart.Id, item.ProductCatalogId,
                    item.ProductName, item.ProductDescription, item.Price.Amount, item.Price.Currency);
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

            await conn.ExecuteAsync(
                addAllForShoppingCartSql,
                items,
                tx).ConfigureAwait(false);
        }
    }
}
