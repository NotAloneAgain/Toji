using Exiled.API.Features;
using PlayerRoles;

namespace Toji.Classes.API.Features.SpawnRules
{
    public class TeamSpawnRules : SpawnRules<Team>
    {
        public TeamSpawnRules(Team value) : base(value) { }

        public TeamSpawnRules(Team value, RoleTypeId model) : base(value, model) { }

        public bool Check(RoleTypeId role) => Check(role.GetTeam()) && role is not RoleTypeId.FacilityGuard and not RoleTypeId.Scp0492;

        public override bool Check(Team value) => value == Value;

        public override bool Check(Player player) => Check(player.Role.Team) && player.Role.Type is not RoleTypeId.FacilityGuard and not RoleTypeId.Scp0492;
    }
}
