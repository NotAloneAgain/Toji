using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Group
{
    public class Thief : DGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        public override string Name => "Грабитель";

        public override string Desc => "Грабил банки и частные дома, являешься международным экспертом во взломе и проникновении";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new StealAbility(35, false, 9, 30, 5.08f, new List<ItemType>(11)
            {
                ItemType.ParticleDisruptor,
                ItemType.MicroHID,
                ItemType.GunLogicer,
                ItemType.ArmorCombat,
                ItemType.ArmorHeavy,
                ItemType.ArmorLight,
                ItemType.GunShotgun,
                ItemType.GunE11SR,
                ItemType.GunCrossvec,
                ItemType.SCP244a,
                ItemType.SCP244b
            })
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new InventoryCharacteristic(new List<Slot>(4)
            {
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.KeycardJanitor, 50 },
                    { ItemType.Coin, 100 }
                }),
                new StaticSlot(ItemType.Medkit),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.ArmorLight, 25 },
                    { ItemType.Medkit, 25 },
                    { ItemType.Radio, 35 },
                    { ItemType.Painkillers, 100 }
                }),
            })
        };

        public int Chance => 12;

        public int Max => 3;
    }
}
