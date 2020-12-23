// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicroservices
// FILE:  IProductCatalogClient.cs
// AUTHOR:  Greg Eakin

using System.Collections.Generic;
using System.Threading.Tasks;
using HelloMicroservices.Models;

namespace HelloMicroservices
{
    public interface IProductCatalogClient
    {
        Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds);
    }
}