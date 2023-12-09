using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Group
{
    public class Collector : DGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "Коллекционер";

        public override string Desc => "Любитель подбирать порой странные и ненужные предметы";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new TradeAbility(40),
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new InventoryCharacteristic(new List<Slot>(4)
            {
                new RandomSlot(new Dictionary<ItemType, int>(6)
                {
                    { ItemType.KeycardJanitor, 10 },
                    { ItemType.Medkit, 15 },
                    { ItemType.Adrenaline, 20 },
                    { ItemType.Painkillers, 25 },
                    { ItemType.Coin, 30 },
                    { ItemType.Flashlight, 35 }
                }),
                new RandomSlot(new Dictionary<ItemType, int>(5)
                {
                    { ItemType.KeycardScientist, 3 },
                    { ItemType.SCP500, 6 },
                    { ItemType.Adrenaline, 9 },
                    { ItemType.Coin, 12 },
                    { ItemType.Flashlight, 15 }
                }),
                new RandomSlot(new Dictionary<ItemType, int>(4)
                {
                    { ItemType.Radio, 2 },
                    { ItemType.Adrenaline, 4 },
                    { ItemType.Coin, 6 },
                    { ItemType.Flashlight, 8 }
                }),
                new RandomSlot(new Dictionary<ItemType, int>(3)
                {
                    { ItemType.ArmorLight, 1 },
                    { ItemType.Coin, 2 },
                    { ItemType.Flashlight, 3 }
                }),
            })
        };

        public int Chance => 15;

        public int Max => 2;
    }
}
