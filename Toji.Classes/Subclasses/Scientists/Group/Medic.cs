using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scientists.Group
{
    public class Medic : ScientistGroupSubclass, ILimitableGroup, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Врач";

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

        protected internal override void OnEscaped(in Player player)
        {
            if (!TryGet("Медик", out var subclass) || !subclass.Can(player))
            {
                base.OnEscaped(player);

                return;
            }

            for (int i = 0; i < 7; i++)
            {
                player.RemoveItem(player.Items.ElementAt(i));
            }

            Revoke(player);

            subclass.Assign(player);
        }
    }
}
