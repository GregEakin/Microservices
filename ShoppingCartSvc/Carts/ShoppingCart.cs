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

            if (eventStore == null)
                return;
            
            foreach (var item in shoppingCartItems)
            {
                var added = _items.Add(item);
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
