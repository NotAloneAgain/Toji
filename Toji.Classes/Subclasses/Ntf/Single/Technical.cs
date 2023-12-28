using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Relations;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Ntf.Single
{
    public class Technical : NtfSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Технический специалист";

        public override RoleTypeId Model => RoleTypeId.NtfSergeant;

        public override string Desc => "Технический специалист, готовый бороться за комплекс против SCP-079";

        public override List<string> Tags { get; } = new List<string>(1) { "Shadow" };

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(2)
        {
            new HackAbility(190),
            new DisableAbility(200),
        };

        public override List<BaseRelation> Relations { get; } = new List<BaseRelation>(1)
        {
            new RoleRelation(RelationType.Required, RoleTypeId.Scp079)
        };

        public int Chance => 12;
    }
}
