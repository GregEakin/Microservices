// Copyright 2020 Greg Eakin
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
// SUBSYSTEM: ShoppingCart
// FILE:  ShoppingCartItem.cs
// AUTHOR:  Greg Eakin

namespace ShoppingCartSvc.Carts
{
    public class ShoppingCartItem
    {
        public int ProductCatalogId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public Money Price { get; set; }

        public ShoppingCartItem() { }

        public ShoppingCartItem(
            int productCatalogId,
            string productName,
            string productDescription,
            Money price)
        {
            ProductCatalogId = productCatalogId;
            ProductName = productName;
            ProductDescription = productDescription;
            Price = price ?? new Money("none", 7m);
            if (string.IsNullOrEmpty(Price.Currency))
                Price.Currency = "Ukn";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var that = obj as ShoppingCartItem;
            return ProductCatalogId.Equals(that?.ProductCatalogId);
        }

        public override int GetHashCode()
        {
            return ProductCatalogId.GetHashCode();
        }
    }
}
