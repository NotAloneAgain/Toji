using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scientists.Single
{
    public class Infected : ScientistSingleSubclass, IHintSubclass, IRandomSubclass, INeedRole
    {
        public override bool ShowInfo => false;

        public override string Name => "Зараженный";

        public override string Desc => "Он явно понимает, что его последние дни сочтены";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(2)
        {
            new AmogusAbility(0, RoleTypeId.Scp0492, RoleTypeId.Scientist),
            new ReviveAbility(RoleTypeId.Scp0492, RoleTypeId.Scientist)
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new HealthCharacteristic(80),
            new InventoryCharacteristic(new List<Slot>(1)
            {
                new StaticSlot(ItemType.KeycardScientist),
            })
        };

        public RoleTypeId NeedRole => RoleTypeId.Scp049;

        public int Chance => 16;
    }
}
