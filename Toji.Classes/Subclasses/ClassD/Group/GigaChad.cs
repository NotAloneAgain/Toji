using Exiled.API.Enums;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;
using UnityEngine;

namespace Toji.Classes.Subclasses.ClassD.Group
{
    public class GigaChad : DGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        public override string Name => "Гигант";

        public override string Desc => "Более высокий и крепкий чем другие, зачастую использовался для переноса грузов";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(2)
        {
            new KnockAbility(110, 5, new HashSet<DoorType>()
            {
                DoorType.LczArmory,
                DoorType.HczArmory,
                DoorType.HID,
            }),
            new StealAbility(50, true, 20, 100, 3.76f, new List<ItemType>(8)
            {
                ItemType.Jailbird,
                ItemType.GunFRMG0,
                ItemType.GunLogicer,
                ItemType.ParticleDisruptor,
                ItemType.MicroHID,
                ItemType.ArmorCombat,
                ItemType.ArmorHeavy,
                ItemType.ArmorLight
            })
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new SizeCharacteristic(Vector3.one * 1.12f),
            new HealthCharacteristic(150)
        };

        public int Max => 3;

        public int Chance => 16;
    }
}
