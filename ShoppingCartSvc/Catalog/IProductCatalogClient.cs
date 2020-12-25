// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  IProductCatalogClient.cs
// AUTHOR:  Greg Eakin

using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartSvc.Models;

namespace ShoppingCartSvc
{
    public interface IProductCatalogClient
    {
        Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds);
    }
}