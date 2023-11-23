using System.Collections.Generic;
using UnityEngine;

namespace Toji.Classes.API.Features.Inventory
{
    public readonly struct Slot
    {
        private readonly IDictionary<ItemType, int> _items;

        public Slot(IDictionary<ItemType, int> items) => _items = items;

        public ItemType GetRandomItem()
        {
            foreach (var item in _items)
            {
                if (Random.Range(0, 101) < item.Value)
                {
                    continue;
                }

                return item.Key;
            }

            return ItemType.None;
        }
    }
}
