using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Single
{
    public class Gang : DSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Блатной";

        public override string Desc => "Уважаем и почитаем всем D-Персоналом";

        public int Chance => 15;

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(120, 4.6f)
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(3)
        {
            new HealthCharacteristic(125),
            new InventoryCharacteristic(new List<Slot>(2)
            {
                new StaticSlot(ItemType.Adrenaline),
                new StaticSlot(ItemType.Painkillers),
            })
        };
    }
}
