// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ShoppingCartSvc
// FILE:  ProductCatalogProduct.cs
// AUTHOR:  Greg Eakin

using ShoppingCartSvc.Carts;

namespace ShoppingCartSvc.Catalog
{
    public class ProductCatalogProduct
    {
        public ProductCatalogProduct(string productId, string productName, string productDescription, Money price)
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            Price = price;
        }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public Money Price { get; set; }
    }
}