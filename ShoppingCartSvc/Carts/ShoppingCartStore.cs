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
            var items = await conn.QueryAsync<ShoppingCartItem, Money, ShoppingCartItem>(readItemsSql,
                (shoppingCartItem, money) => { shoppingCartItem.Price = money; return shoppingCartItem; },
                new { UserId = userId },
                splitOn: "Currency");
            return new ShoppingCart(userId, items);
        }

        private const string deleteAllForShoppingCartSql =
            @"delete item from ShoppingCartItems item
inner join ShoppingCart cart on item.ShoppingCartId = cart.ID
and cart.UserId=@UserId";

        private const string addAllForShoppingCartSql =
            @"insert into ShoppingCartItems 
(ShoppingCartId, ProductCatalogId, ProductName, 
ProductDescription, Amount, Currency)
values 
(@ShoppingCartId, @ProductCatalogId, @ProductName,v
@ProductDescription, @Amount, @Currency)";

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
