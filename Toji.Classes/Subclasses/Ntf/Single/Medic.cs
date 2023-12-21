using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Ntf.Single
{
    public class Medic : NtfSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Медик";

        public override RoleTypeId Role => RoleTypeId.NtfSergeant;

        public override string Desc => "Профессиональный полевой врач, способный помочь всем и всюду";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new HealAbility(48, 5, 0.3f, 50)
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(3)
        {
            new InventoryCharacteristic(new List<Slot>(8)
            {
                new StaticSlot(ItemType.KeycardMTFOperative),
                new StaticSlot(ItemType.GunE11SR),
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
                new StaticSlot(ItemType.GrenadeFlash),
                new StaticSlot(ItemType.Radio),
                new StaticSlot(ItemType.ArmorCombat),
            })
        };

        public int Chance => 16;
    }
}
