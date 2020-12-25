// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ProductCatalogSvc
// FILE:  ProductCatalogProduct.cs
// AUTHOR:  Greg Eakin

namespace ProductCatalogSvc.Products
{
    public class ProductCatalogProduct
    {
        public ProductCatalogProduct(int productId, string productName, string description, Money price)
        {
            ProductId = productId.ToString();
            ProductName = productName;
            ProductDescription = description;
            Price = price;
        }
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }
        public Money Price { get; private set; }
    }
}