using PlayerRoles;
using Toji.Classes.API.Features.SpawnRules;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Subclasses;

namespace Toji.Classes.Subclasses.Chaos
{
    public abstract class ChaosGroupSubclass : GroupSubclass
    {
        private TeamSpawnRules _rules;

        public abstract RoleTypeId Model { get; }

        public override BaseSpawnRules SpawnRules => _rules ??= new TeamSpawnRules(Team.ChaosInsurgency, Model);
    }
}
