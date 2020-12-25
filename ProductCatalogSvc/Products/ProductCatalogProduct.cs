// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: ProductCatalogSvc
// FILE:  ProductCatalogProduct.cs
// AUTHOR:  Greg Eakin

using System.Text.Json.Serialization;

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

        [JsonPropertyName("productId")]
        public string ProductId { get; private set; }
        
        [JsonPropertyName("productName")]
        public string ProductName { get; private set; }
        
        [JsonPropertyName("productDescription")]
        public string ProductDescription { get; private set; }

        [JsonPropertyName("price")]
        public Money Price { get; private set; }
    }
}