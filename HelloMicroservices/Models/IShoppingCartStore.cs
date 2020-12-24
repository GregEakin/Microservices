// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicroservices
// FILE:  IShoppingCartStore.cs
// AUTHOR:  Greg Eakin

using System.Threading.Tasks;

namespace HelloMicroservices.Models
{
    public interface IShoppingCartStore
    {
        Task<ShoppingCart> Get(int userId);
        Task Save(ShoppingCart shoppingCart);
    }
}