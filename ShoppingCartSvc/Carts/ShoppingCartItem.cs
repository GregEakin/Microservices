// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCart
// FILE:  ShoppingCartItem.cs
// AUTHOR:  Greg Eakin

namespace ShoppingCartSvc.Carts
{
    public class ShoppingCartItem
    {
        public int ProductCatalogId { get; }
        public string ProductName { get; }
        public string ProductDescription { get; }
        public Money Price { get; }

        public ShoppingCartItem(
            int productCatalogId,
            string productName,
            string productDescription,
            Money price)
        {
            this.ProductCatalogId = productCatalogId;
            this.ProductName = productName;
            this.ProductDescription = productDescription;
            this.Price = price;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var that = obj as ShoppingCartItem;
            return this.ProductCatalogId.Equals(that.ProductCatalogId);
        }

        public override int GetHashCode()
        {
            return this.ProductCatalogId.GetHashCode();
        }
    }
}