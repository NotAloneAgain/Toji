using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Guards.Group
{
    public class Assault : GuardGroupSubclass, IHintSubclass, ILimitableGroup, IPrioritySubclass
    {
        public override string Name => "Штурмовик";

        public override string Desc => "Закалённый в боях сотрудник, готовый к большинству ситуаций";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(100, 4.8f)
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new HealthCharacteristic(125),
            new InventoryCharacteristic(new List<Slot>(8)
            {
                new StaticSlot(ItemType.KeycardGuard),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.GunFRMG0, 43 },
                    { ItemType.GunFSP9, 100 },
                }),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.GrenadeHE, 30 },
                    { ItemType.GrenadeFlash, 100 },
                }),
                new StaticSlot(ItemType.Medkit),
                new StaticSlot(ItemType.Radio),
                new StaticSlot(ItemType.ArmorCombat),
            })
        };

        public LoadPriority Priority => LoadPriority.Lowest;

        public int Max => 2;
    }
}
