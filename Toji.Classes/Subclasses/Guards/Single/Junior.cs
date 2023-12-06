using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Guards.Single
{
    public class Junior : GuardSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "Младший сотрудник";

        public override string Desc => "Недавно устроившись в Фонд, ты ещё не успел получить свою светошумовую гранату";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new InventoryCharacteristic(new List<Slot>(8)
            {
                new StaticSlot(ItemType.KeycardGuard),
                new StaticSlot(ItemType.GunFSP9),
                new StaticSlot(ItemType.Medkit),
                new StaticSlot(ItemType.Radio),
                new StaticSlot(ItemType.ArmorLight),
            })
        };

        public int Chance => 12;
    }
}
