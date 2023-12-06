using Exiled.API.Features;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class InventoryCharacteristic : Characteristic<List<Slot>>
    {
        public InventoryCharacteristic(List<Slot> value) : base(value) { }

        public override string Name => "Свой инвентарь";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            foreach (var slot in Value)
            {
                var item = slot.GetItem();

                if (item == ItemType.None)
                    continue;

                player.AddItem(item);
            }
        }

        public override string GetDesc(Player player = null) => "Вы имеете свой уникальный инвентарь";
    }
}
