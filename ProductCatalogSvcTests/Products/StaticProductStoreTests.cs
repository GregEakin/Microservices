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

using NUnit.Framework;
using ProductCatalogSvc.Products;
using System.Linq;

namespace ProductCatalogSvcTests.Products
{
    public class StaticProductStoreTests
    {
        [Test]
        public void Single_CreateProductTest()
        {
            var store = new StaticProductStore();
            var product = store.GetProductsByIds(new[] { 0 }).Single();
            Assert.That("0", Is.EqualTo(product.ProductId));
            Assert.That(product.ProductName, Is.Not.Null);
            Assert.That(product.ProductDescription, Is.Not.Null);
            Assert.That(product.Price, Is.Not.Null);
        }

        [Test]
        public void Several_CreateProductsTest()
        {
            var store = new StaticProductStore();
            var products = store.GetProductsByIds(new[] { 2, 6, 9 }).ToArray();
            Assert.That(3, Is.EqualTo(products.Length));
            Assert.That("2", Is.EqualTo(products[0].ProductId));
            Assert.That("6", Is.EqualTo(products[1].ProductId));
            Assert.That("9", Is.EqualTo(products[2].ProductId));
        }

        [Test]
        public void Negative_CreateProductsTest()
        {
            var store = new StaticProductStore();
            var products = store.GetProductsByIds(new[] { -1 });
            Assert.That(products.Any(), Is.False);
        }

        [Test]
        public void TooBig_CreateProductsTest()
        {
            var store = new StaticProductStore();
            var products = store.GetProductsByIds(new[] { 200 });
            Assert.That(products.Any(), Is.False);
        }
    }
}