// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  IProductCatalogClient.cs
// AUTHOR:  Greg Eakin

using ShoppingCartSvc.Carts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartSvc.Catalog
{
    public interface IProductCatalogClient
    {
        Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds);
    }
}