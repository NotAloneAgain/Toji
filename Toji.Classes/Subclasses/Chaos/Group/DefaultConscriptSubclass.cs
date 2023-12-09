using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Chaos.Group
{
    public class DefaultConscriptSubclass : ChaosGroupSubclass
    {
        public override string Name => "Новобранец";

        public override RoleTypeId Role => RoleTypeId.ChaosConscript;

        public override string Desc => "Стандартный новобранец повстанцев хаоса";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(100, 5)
        };
    }
}
