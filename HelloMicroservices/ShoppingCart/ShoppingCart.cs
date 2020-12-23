using HelloMicroservices.EventFeed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloMicroservices.ShoppingCart
{
    public class ShoppingCart
    {
        private readonly HashSet<ShoppingCartItem> _items = new();

        public int UserId { get; }

        public IEnumerable<ShoppingCartItem> Items => _items;

        public ShoppingCart(int userId)
        {
            UserId = userId;
        }

        public ShoppingCart(int userId, IEnumerable<ShoppingCartItem> items)
        {
            UserId = userId;
            foreach (var item in items)
                _items.Add(item);
        }

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            foreach (var item in shoppingCartItems)
            {
                var added = _items.Add(item);
                if (added)
                    eventStore.Raise("ShoppingCartItemAdded", new { UserId, item });
            }
        }

        public void RemoveItems(int[] productCatalogIds, IEventStore eventStore)
        {
            var count = _items.RemoveWhere(i => productCatalogIds.Contains(i.ProductCatalogId));
            foreach (var item in productCatalogIds)
                eventStore.Raise("ShoppingCartItemRemoved", new { UserId, item });
        }
    }
}
