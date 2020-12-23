// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicroservices
// FILE:  ShoppingCartStore.cs
// AUTHOR:  Greg Eakin

using System.Collections.Generic;

namespace HelloMicroservices.Models
{
    public class ShoppingCartStore : IShoppingCartStore
    {
        private static readonly Dictionary<int, ShoppingCart> Database = new();

        public ShoppingCart Get(int userId)
        {
            if (!Database.ContainsKey(userId))
                Database[userId] = new ShoppingCart(userId);

            return Database[userId];
        }

        public void Save(ShoppingCart shoppingCart)
        {
            // Nothing needed. Saving would be needed with a real DB
        }
    }
}