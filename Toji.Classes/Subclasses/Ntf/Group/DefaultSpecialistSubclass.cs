using Exiled.API.Enums;
using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Ntf.Group
{
    public class DefaultSpecialistSubclass : NtfGroupSubclass, IPrioritySubclass
    {
        public override bool ShowInfo => false;

        public override string Name => "Специалист";

        public override RoleTypeId Role => RoleTypeId.NtfSpecialist;

        public override string Desc => "Стандартный специалист девятихвостой лисы";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(110, 5, new HashSet<DoorType>()
            {
                DoorType.LczArmory,
                DoorType.HczArmory,
                DoorType.HID,
            })
        };

        public LoadPriority Priority => LoadPriority.Last;
    }
}
