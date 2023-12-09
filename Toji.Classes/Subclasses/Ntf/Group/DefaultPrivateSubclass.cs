using Exiled.API.Enums;
using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Ntf.Group
{
    public class DefaultPrivateSubclass : NtfGroupSubclass, IPrioritySubclass
    {
        public override string Name => "Рядовой";

        public override RoleTypeId Role => RoleTypeId.NtfPrivate;

        public override string Desc => "Стандартный рядовой девятихвостой лисы";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(90, 3.8f)
        };

        public LoadPriority Priority => LoadPriority.Last;
    }
}
