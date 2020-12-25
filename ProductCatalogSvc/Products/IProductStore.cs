// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ProductCatalogSvc
// FILE:  IProductStore.cs
// AUTHOR:  Greg Eakin

using System.Collections.Generic;

namespace ProductCatalogSvc.Products
{
    public interface IProductStore
    {
        IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds);
    }
}