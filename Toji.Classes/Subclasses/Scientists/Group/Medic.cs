using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scientists.Group
{
    public class Medic : ScientistGroupSubclass, ILimitableGroup, IHintSubclass, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "Медик";

        public override string Desc => "Профессиональный врач, готовый работать со множеством ран";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new HealAbility(60)
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(3)
        {
            new HealthCharacteristic(125),
            new InventoryCharacteristic(new List<Slot>(4)
            {
                new StaticSlot(ItemType.KeycardScientist),
                new StaticSlot(ItemType.Medkit),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.Medkit, 50 },
                    { ItemType.Painkillers, 100 },
                }),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.Medkit, 25 },
                    { ItemType.Painkillers, 100 },
                }),
            })
        };

        public int Chance => 16;

        public int Max => 3;
    }
}
