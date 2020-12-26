﻿// Copyright 2020 Greg Eakin
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at:
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
// SUBSYSTEM: ProductCatalogSvc
// FILE:  StaticProductStore.cs
// AUTHOR:  Greg Eakin

using System.Collections.Generic;

namespace ProductCatalogSvc.Products
{
    public class StaticProductStore : IProductStore
    {
        public IEnumerable<ProductCatalogProduct> GetProductsByIds(IEnumerable<int> productIds)
        {
            const int maxProductId = 200;
            
            foreach (var id in productIds)
            {
                if (id < 0 || id >= maxProductId)
                    continue;
                
                yield return new ProductCatalogProduct(id, "foo" + id, "bar", new Money());
            }
        }
    }
}