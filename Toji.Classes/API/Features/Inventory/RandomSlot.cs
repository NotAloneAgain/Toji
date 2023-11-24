using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toji.Classes.API.Features.Inventory
{
    public class RandomSlot : Slot
    {
        private readonly IDictionary<ItemType, int> _items;

        public RandomSlot(IDictionary<ItemType, int> items) => _items = items;

        public RandomSlot(params KeyValuePair<ItemType, int>[] pairs)
        {
            _items = pairs.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public RandomSlot(IEnumerable<KeyValuePair<ItemType, int>> pairs)
        {
            _items = pairs.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public override ItemType GetItem()
        {
            foreach (var item in _items)
            {
                if (Random.Range(0, 100) < item.Value)
                {
                    continue;
                }

                return item.Key;
            }

            return ItemType.None;
        }
    }
}
