using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scientists.Group
{
    public class CandyMan : ScientistGroupSubclass, ILimitableGroup, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Любитель конфет";

        public override string Desc => "Сладкоежка, готовый на все, лишь бы получить побольше SCP-330";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new MoreCandyAbility(3)
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(3)
        {
            new HealthCharacteristic(125),
            new InventoryCharacteristic(new List<Slot>(3)
            {
                new StaticSlot(ItemType.KeycardScientist),
                new StaticSlot(ItemType.Medkit),
                new StaticSlot(ItemType.SCP330),
            })
        };

        public int Chance => 15;

        public int Max => 2;
    }
}
