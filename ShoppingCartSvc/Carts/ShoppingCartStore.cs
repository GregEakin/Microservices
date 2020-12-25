// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  ShoppingCartStore.cs
// AUTHOR:  Greg Eakin

using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace ShoppingCartSvc.Carts
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private const string connectionString =
            @"Host=microservices_cartdb_1;Port=5432;Database=cartapp;Username=cartapp;Password=cartpw";

        private const string readItemsSql =
            @"select ""ProductCatalogId"", ""ProductName"", ""ProductDescription"", ""Currency"", ""Amount""
from ""public"".""ShoppingCart"", ""public"".""ShoppingCartItems""
where ""ShoppingCart"".""id"" = ""ShoppingCartItems"".""ShoppingCartId""
and ""ShoppingCart"".""UserId"" = @UserId";

        public async Task<ShoppingCart> Get(int userId)
        {
            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            var items = await conn.QueryAsync<ShoppingCartItem, Money, ShoppingCartItem>(readItemsSql,
                (shoppingCartItem, money) =>
                {
                    if (shoppingCartItem.Price == null) 
                        return shoppingCartItem;
                    
                    shoppingCartItem.Price.Amount = money.Amount;
                    shoppingCartItem.Price.Currency = money.Currency;
                    return shoppingCartItem;
                },
                new { UserId = userId },
                splitOn: "Currency");
            return new ShoppingCart(userId, items);
        }

        // DELETE FROM table_name WHERE condition RETURNING(select_list | *)
        // DELETE FROM t1 USING t2 WHERE t1.id = t2.id

        private const string deleteAllForShoppingCartSql = @"DELETE FROM ""ShoppingCartItems"" AS t1 USING ""ShoppingCart"" AS t2 WHERE t1.""ShoppingCartId"" = t2.id AND t2.""UserId"" = @UserId";

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
                new { UserId = shoppingCart.UserId },
                tx).ConfigureAwait(false);
            await conn.ExecuteAsync(
                addAllForShoppingCartSql,
                shoppingCart.Items,
                tx).ConfigureAwait(false);
        }
    }
}
