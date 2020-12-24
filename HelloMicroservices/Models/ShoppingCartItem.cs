// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicroservices
// FILE:  ShoppingCartItem.cs
// AUTHOR:  Greg Eakin

namespace HelloMicroservices.Models
{
    public class ShoppingCartItem
    {
        public int ProductCatalogId { get; set;  }
        public string ProductName { get; set;  } 
        public string ProductDescription { get; set; }
        public Money Price { get; set; }

        public ShoppingCartItem()
        {
        }
        
        public ShoppingCartItem(int productCatalogId, string productName, string productDescription, Money price)
        {
            ProductCatalogId = productCatalogId;
            ProductName = productName;
            ProductDescription = productDescription;
            Price = price;
        }
    }
}