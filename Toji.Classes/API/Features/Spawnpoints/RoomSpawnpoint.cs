using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Toji.Classes.API.Extensions;
using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class RoomSpawnpoint : BaseSpawnpoint
    {
        public RoomSpawnpoint(RoomType room) => Type = room;

        public RoomType Type { get; init; }

        public sealed override Vector3 Position => Room.Get(room => room.Type == Type).GetRandomValue().GetSafePosition() + Vector3.up * 2;
    }
}
