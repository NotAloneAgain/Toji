using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Guards.Group
{
    public class Sniper : GuardGroupSubclass, IHintSubclass, IRandomSubclass
    {
        public override bool ShowInfo => false;

        public override string Name => "Снайпер";

        public override string Desc => "Очень точный пользователь винтовки, получивший её в целях усиления охраны";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new InventoryCharacteristic(new List<Slot>(8)
            {
                new StaticSlot(ItemType.KeycardGuard),
                new StaticSlot(ItemType.GunE11SR),
                new StaticSlot(ItemType.Adrenaline),
                new StaticSlot(ItemType.Medkit),
                new StaticSlot(ItemType.Radio),
                new StaticSlot(ItemType.ArmorLight),
            })
        };

        public int Chance => 18;
    }
}
