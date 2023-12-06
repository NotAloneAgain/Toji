using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Guards.Single
{
    public class Senior : GuardSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "Глава охраны";

        public override string Desc => "Опытный руководитель охраной комплекса, проверенный временем и боями";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(110, 5)
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new DamageMultiplayerCharacteristic(1.15f, new List<RoleTypeId>()
            {
                RoleTypeId.ChaosMarauder,
                RoleTypeId.ChaosRepressor,
                RoleTypeId.ChaosRifleman,
                RoleTypeId.ChaosConscript
            }),
            new InventoryCharacteristic(new List<Slot>(8)
            {
                new StaticSlot(ItemType.KeycardMTFPrivate),
                new StaticSlot(ItemType.GunCrossvec),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.GrenadeHE, 50 },
                    { ItemType.GrenadeFlash, 100 },
                }),
                new StaticSlot(ItemType.Medkit),
                new StaticSlot(ItemType.Adrenaline),
                new StaticSlot(ItemType.Radio),
                new StaticSlot(ItemType.ArmorCombat),
            })
        };

        public int Chance => 16;
    }
}
