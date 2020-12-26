// Copyright © 2020-2020. All Rights Reserved.
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
            return this.ProductCatalogId.Equals(that.ProductCatalogId);
        }

        public override int GetHashCode()
        {
            return this.ProductCatalogId.GetHashCode();
        }
    }
}
