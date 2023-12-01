using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Doors;
using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class DoorSpawnpoint : BaseSpawnpoint
    {
        public DoorSpawnpoint(DoorType door) => Type = door;

        public DoorSpawnpoint(DoorType door, Vector3 offset) : this(door) => Offset = offset;

        public DoorType Type { get; init; }

        public Vector3 Offset { get; init; }

        public sealed override Vector3 Position
        {
            get
            {
                var door = Door.Get(door => door.Type == Type).GetRandomValue();

                if (Offset != Vector3.zero && Offset != Vector3.one)
                {
                    return door.Position + Vector3.up * 2 + door.Room.Rotation * Offset;
                }

                return door.Position + Vector3.up;
            }
        }
    }
}
