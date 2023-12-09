using Exiled.API.Enums;
using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Ntf.Group
{
    public class DefaultPrivateSubclass : NtfGroupSubclass
    {
        public override string Name => "Рядовой";

        public override RoleTypeId Role => RoleTypeId.NtfPrivate;

        public override string Desc => "Стандартный рядовой девятихвостой лисы";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(90, 4)
        };
    }
}
