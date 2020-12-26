// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  ShoppingCartStore.cs
// AUTHOR:  Greg Eakin

using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartSvc.Carts
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private const string connectionString =
            @"Host=microservices_cartdb_1;Port=5432;Database=cartapp;Username=cartapp;Password=cartpw";

        private const string readItemsSql =
            @"select ""ShoppingCart"".id, ""ProductCatalogId"", ""ProductName"", ""ProductDescription"", ""Currency"", ""Amount""
from ""ShoppingCart"" left join ""ShoppingCartItems"" on ""ShoppingCart"".""id"" = ""ShoppingCartItems"".""ShoppingCartId""
where ""ShoppingCart"".""UserId"" = @UserId";

        public async Task<ShoppingCart> Get(int userId)
        {
            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            ShoppingCart shoppingCart = null;
            var items = await conn.QueryAsync<int, ShoppingCartItem, Money, ShoppingCart>(readItemsSql,
                (id, shoppingCartItem, money) =>
                {
                    Console.WriteLine("Read DB: {0}, {1}, {2}, {3}, {4}, {5}", id, shoppingCartItem?.ProductCatalogId,
                        shoppingCartItem?.ProductName, shoppingCartItem?.ProductDescription, 
                        money?.Amount, money?.Currency);

                    shoppingCart ??= new ShoppingCart(id, new ShoppingCartItem[]{});
                    if (shoppingCartItem == null)
                        return shoppingCart;
                    
                    shoppingCart.AddItems(new[] {shoppingCartItem}, null);

                    if (money == null)
                    {
                        Console.WriteLine("Money is null!");
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

        private const string deleteAllForShoppingCartSql = @"DELETE FROM ""ShoppingCartItems"" WHERE ""ShoppingCartId"" = @Id";

        private const string addAllForShoppingCartSql =
            @"insert into ""ShoppingCartItems"" (""ShoppingCartId"", ""ProductCatalogId"", ""ProductName"", ""ProductDescription"", ""Amount"", ""Currency"")
values 
(@ShoppingCartId, @ProductCatalogId, @ProductName, @ProductDescription, @Amount, @Currency)";

        public async Task Save(ShoppingCart shoppingCart)
        {
            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            await using var tx = await conn.BeginTransactionAsync();
            await conn.ExecuteAsync(
                deleteAllForShoppingCartSql,
                new { Id = shoppingCart.Id },
                tx).ConfigureAwait(false);

            foreach (var item in shoppingCart.Items)
            {
                item.Price ??= new Money("none", 1m);
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
            
            await tx.CommitAsync();
        }
    }
}
