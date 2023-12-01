using Exiled.API.Extensions;
using PlayerRoles;
using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class RoleSpawnpoint : BaseSpawnpoint
    {
        public RoleSpawnpoint(RoleTypeId role) => Type = role;

        public RoleTypeId Type { get; init; }

        public sealed override Vector3 Position => Type.GetRandomSpawnLocation().Position;
    }
}
