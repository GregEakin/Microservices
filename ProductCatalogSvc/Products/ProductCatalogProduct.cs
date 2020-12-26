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