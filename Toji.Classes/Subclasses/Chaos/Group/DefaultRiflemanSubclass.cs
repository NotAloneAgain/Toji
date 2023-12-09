using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Chaos.Group
{
    public class DefaultRiflemanSubclass : ChaosGroupSubclass
    {
        public override string Name => "Стрелок";

        public override RoleTypeId Role => RoleTypeId.ChaosRifleman;

        public override string Desc => "Стандартный стрелок повстанцев хаоса";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(80, 4)
        };
    }
}
