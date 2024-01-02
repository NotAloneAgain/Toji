using Exiled.API.Extensions;
using PlayerRoles;
using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class RoleSpawnpoint(RoleTypeId role) : BaseSpawnpoint
    {
        public RoleTypeId Type { get; init; } = role;

        public sealed override Vector3 Position => Type.GetRandomSpawnLocation().Position;
    }
}
