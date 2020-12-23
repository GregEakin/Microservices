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

            if (userId != 42) return;
            var item1 = new ShoppingCartItem(1, "Basic t-shirt", "a quiet t-shirt", new Money("eur", "40"));
            var item2 = new ShoppingCartItem(2, "Fancy shirt", "a loud t-shirt", new Money("eur", "50"));
            _items.Add(item1);
            _items.Add(item2);

        }

        public ShoppingCart(int userId, IEnumerable<ShoppingCartItem> items)
        {
            if (items == null)
                return;
            
            UserId = userId;
            foreach (var item in items)
                _items.Add(item);
        }

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            if (shoppingCartItems == null)
                return;

            if (eventStore == null)
                return;
            
            foreach (var item in shoppingCartItems)
            {
                var added = _items.Add(item);
                if (added)
                    eventStore.Raise("ShoppingCartItemAdded", new { UserId, item });
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
                eventStore.Raise("ShoppingCartItemRemoved", new { UserId, item });
        }
    }
}
