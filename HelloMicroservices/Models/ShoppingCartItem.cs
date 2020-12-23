// Copyright © 2020-2020. All Rights Reserved.
// 
// SUBSYSTEM: HelloMicroservices
// FILE:  ShoppingCartItem.cs
// AUTHOR:  Greg Eakin

namespace HelloMicroservices.Models
{
    public record ShoppingCartItem(int ProductCatalogId, string ProductName, string Description, Money Price);
}