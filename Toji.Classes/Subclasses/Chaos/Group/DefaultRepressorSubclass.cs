using Exiled.API.Enums;
using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Chaos.Group
{
    public class DefaultRepressorSubclass : ChaosGroupSubclass, IPrioritySubclass
    {
        public override bool ShowInfo => false;

        public override string Name => "Усмиритель";

        public override RoleTypeId Model => RoleTypeId.ChaosRepressor;

        public override string Desc => "Стандартный усмиритель повстанцев хаоса";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(120, 5, new HashSet<DoorType>()
            {
                DoorType.HID,
            })
        };

        public LoadPriority Priority => LoadPriority.Last;
    }
}
