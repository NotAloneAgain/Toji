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

        public override string Desc => "Ты грабил банки и частные дома, международный эксперт в взломе и проникновении";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new StealAbility(35, false, 9, 30, 5.08f, ItemType.ParticleDisruptor, ItemType.MicroHID, ItemType.GunLogicer, ItemType.ArmorCombat, ItemType.ArmorHeavy, ItemType.ArmorLight, ItemType.GunShotgun, ItemType.GunE11SR, ItemType.GunCrossvec, ItemType.SCP244a, ItemType.SCP244b)
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new InventoryCharacteristic(new List<Slot>(4)
            {
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.SCP500, 1 },
                    { ItemType.KeycardJanitor, 5 },
                    { ItemType.Medkit, 10 },
                    { ItemType.Painkillers, 30 },
                    { ItemType.Coin, 80 }
                }),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.Painkillers, 20 },
                    { ItemType.Coin, 60 }
                }),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.Painkillers, 10 },
                    { ItemType.Coin, 40 }
                }),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.ArmorLight, 3 },
                    { ItemType.Painkillers, 5 },
                    { ItemType.Coin, 20 }
                }),
            })
        };

        public int Max => 3;

        public int Chance => 12;
    }
}
