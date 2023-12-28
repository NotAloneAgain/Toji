using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using System.Collections.Generic;
using Toji.Classes.API.Features.Characteristics;
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

            player.ClearItems();

            foreach (var slot in Value)
            {
                var item = slot.GetItem();

                if (item == ItemType.None)
                    continue;

                if (player.IsInventoryFull)
                {
                    Pickup.CreateAndSpawn(item, player.Position, player.Rotation, player);
                }
                else
                {
                    player.AddItem(item);
                }
            }
        }

        public override string GetDesc(Player player = null) => "Вы имеете свой уникальный инвентарь";
    }
}
