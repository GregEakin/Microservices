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
// SUBSYSTEM: ShoppingCart
// FILE:  ShoppingCart.cs
// AUTHOR:  Greg Eakin

using ShoppingCartSvc.EventFeed;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartSvc.Carts
{
    public class ShoppingCart
    {
        private readonly HashSet<ShoppingCartItem> _items = new();

        public int Id { get; }

        public IEnumerable<ShoppingCartItem> Items => _items;

        public ShoppingCart(int id, IEnumerable<ShoppingCartItem> items)
        {
            if (items == null)
                return;
            
            Id = id;
            foreach (var item in items)
                _items.Add(item);
        }

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            if (shoppingCartItems == null)
                return;

            foreach (var item in shoppingCartItems)
            {
                var added = _items.Add(item);
                if (eventStore == null)
                    continue;
                if (added)
                    eventStore.Raise("ShoppingCartItemAdded", new { Id = Id, item });
            }
        }

        public void RemoveItems(int[] productCatalogIds, IEventStore eventStore)
        {
            if (productCatalogIds == null)
                return;

            if (eventStore == null)
                return;
            
            var count = _items.RemoveWhere(i => productCatalogIds.Contains(i.ProductCatalogId));
            foreach (var item in productCatalogIds)
                eventStore.Raise("ShoppingCartItemRemoved", new { Id = Id, item });
        }
    }
}
