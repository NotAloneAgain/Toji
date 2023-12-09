using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Ntf.Group
{
    public class DefaultCaptainSubclass : NtfGroupSubclass
    {
        public override string Name => "Капитан";

        public override RoleTypeId Role => RoleTypeId.NtfCaptain;

        public override string Desc => "Стандартный капитан девятихвостой лисы";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(100, 5)
        };
    }
}
