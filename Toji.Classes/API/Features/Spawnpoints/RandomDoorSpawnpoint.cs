using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Doors;
using System.Collections.Generic;
using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class RandomDoorSpawnpoint(IEnumerable<DoorType> doors) : RandomSpawnpoint
    {
        public RandomDoorSpawnpoint(params DoorType[] doors) : this((IEnumerable<DoorType>)doors) { }

        public override Vector3 SelectRandom()
        {
            var type = doors.GetRandomValue();

            var door = Door.Get(door => door.Type == type).GetRandomValue();

            door.IsOpen = true;

            return door.Position + Vector3.up;
        }
    }
}
