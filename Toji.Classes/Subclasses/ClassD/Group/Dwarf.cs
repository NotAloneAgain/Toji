using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.Classes.Subclasses.Characteristics;
using UnityEngine;

namespace Toji.Classes.Subclasses.ClassD.Group
{
    public class Dwarf : DGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        public override string Name => "Карлик";

        public override string Desc => "Имея слабое тело и маленький рост смог выжить и не оказаться попущенным";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(2)
        {
            new DodgeAbility(37, 28, 0.88f),
            new InfinityStaminaAbility()
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(3)
        {
            new SizeCharacteristic(Vector3.one * 0.63f),
            new HealthCharacteristic(50),
            new InventoryCharacteristic(new List<Slot>(2)
            {
                new StaticSlot(ItemType.Medkit),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.Medkit, 50 },
                    { ItemType.Painkillers, 100 }
                }),
            })
        };

        public int Chance => 18;

        public int Max => 4;
    }
}
