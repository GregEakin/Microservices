// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ProductCatalogSvc
// FILE:  StaticProductStore.cs
// AUTHOR:  Greg Eakin

using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogSvc.Products
{
    public class StaticProductStore : IProductStore
    {
        public IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds)
        {
            return productIds.Select(id => new ProductCatalogProduct(id, "foo" + id, "bar", new Money()));
        }
    }
}