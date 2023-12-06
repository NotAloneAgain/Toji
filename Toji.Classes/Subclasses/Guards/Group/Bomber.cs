using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Guards.Group
{
    public class Bomber : GuardGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        public override string Name => "Подрывник";

        public override string Desc => "Любитель взрывов, обязательно найдет способ что-нибудь взорвать";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new InventoryCharacteristic(new List<Slot>(8)
            {
                new StaticSlot(ItemType.KeycardGuard),
                new StaticSlot(ItemType.GunShotgun),
                new StaticSlot(ItemType.GrenadeHE),
                new StaticSlot(ItemType.Medkit),
                new StaticSlot(ItemType.Radio),
                new StaticSlot(ItemType.ArmorLight),
            })
        };

        public int Chance => 18;

        public int Max => 2;
    }
}
