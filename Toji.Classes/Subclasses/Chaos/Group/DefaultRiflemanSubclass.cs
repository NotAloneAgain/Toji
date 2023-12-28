using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.SpawnRules;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;

namespace Toji.Classes.Subclasses.Chaos.Group
{
    public class DefaultRiflemanSubclass : ChaosGroupSubclass, IPrioritySubclass
    {
        public override bool ShowInfo => false;

        public override string Name => "Стрелок";

        public override string Desc => "Стандартный стрелок повстанцев хаоса";

        public sealed override BaseSpawnRules SpawnRules { get; } = new RoleSpawnRules(RoleTypeId.ChaosRifleman);

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new KnockAbility(80, 4)
        };

        public LoadPriority Priority => LoadPriority.Lowest;
    }
}
