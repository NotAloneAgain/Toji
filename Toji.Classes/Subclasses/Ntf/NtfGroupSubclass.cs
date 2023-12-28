using PlayerRoles;
using Toji.Classes.API.Features.SpawnRules;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Subclasses;

namespace Toji.Classes.Subclasses.Ntf
{
    public abstract class NtfGroupSubclass : GroupSubclass
    {
        private TeamSpawnRules _rules;

        public virtual RoleTypeId Model { get; }

        public override BaseSpawnRules SpawnRules => _rules ??= new TeamSpawnRules(Team.FoundationForces, Model);
    }
}
