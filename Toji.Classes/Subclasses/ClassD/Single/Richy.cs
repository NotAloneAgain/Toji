using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Single
{
    public class Richy : DGroupSubclass, IHintSubclass, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "Богатей";

        public override string Desc => "Богатый D-Персонал, делающий ставки на то, кто следующим умрет";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new HealthCharacteristic(120),
            new InventoryCharacteristic(new List<Slot>(6)
            {
                new StaticSlot(ItemType.Coin),
                new StaticSlot(ItemType.Coin),
                new StaticSlot(ItemType.Coin),
                new StaticSlot(ItemType.Coin),
                new StaticSlot(ItemType.Coin),
                new StaticSlot(ItemType.Coin),
            })
        };

        public int Chance => 16;
    }
}
