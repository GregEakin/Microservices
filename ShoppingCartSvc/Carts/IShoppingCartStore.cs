// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  IShoppingCartStore.cs
// AUTHOR:  Greg Eakin

using System.Threading.Tasks;

namespace ShoppingCartSvc.Carts
{
    public interface IShoppingCartStore
    {
        Task<ShoppingCart> Get(int userId);
        Task Save(ShoppingCart shoppingCart);
    }
}