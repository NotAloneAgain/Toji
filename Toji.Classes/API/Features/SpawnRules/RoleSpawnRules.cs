using Exiled.API.Features;
using PlayerRoles;

namespace Toji.Classes.API.Features.SpawnRules
{
    public class RoleSpawnRules : SpawnRules<RoleTypeId>
    {
        public RoleSpawnRules(RoleTypeId value) : base(value, value) { }

        public override bool Check(RoleTypeId value) => value == Value;

        public override bool Check(Player player) => Check(player.Role.Type);
    }
}
