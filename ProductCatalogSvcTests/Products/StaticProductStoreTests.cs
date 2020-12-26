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
// FILE:  UnitTest1.cs
// AUTHOR:  Greg Eakin

using System.Linq;
using NUnit.Framework;
using ProductCatalogSvc.Products;

namespace ProductCatalogSvcTests.Products
{
    public class StaticProductStoreTests
    {
        [Test]
        public void Single_CreateProductTest()
        {
            var store = new StaticProductStore();
            var product = store.GetProductsByIds(new[] { 0 }).Single();
            Assert.AreEqual("0", product.ProductId);
            Assert.IsNotNull(product.ProductName);
            Assert.IsNotNull(product.ProductDescription);
            Assert.IsNotNull(product.Price);
        }

        [Test]
        public void Several_CreateProductsTest()
        {
            var store = new StaticProductStore();
            var products = store.GetProductsByIds(new[] { 2, 6, 9 }).ToArray();
            Assert.AreEqual(3, products.Length);
            Assert.AreEqual("2", products[0].ProductId);
            Assert.AreEqual("6", products[1].ProductId);
            Assert.AreEqual("9", products[2].ProductId);
        }

        [Test]
        public void Negative_CreateProductsTest()
        {
            var store = new StaticProductStore();
            var products = store.GetProductsByIds(new[] { -1 });
            Assert.IsFalse(products.Any());
        }

        [Test]
        public void TooBig_CreateProductsTest()
        {
            var store = new StaticProductStore();
            var products = store.GetProductsByIds(new[] { 200 });
            Assert.IsFalse(products.Any());
        }
    }
}