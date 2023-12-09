using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Doors;
using System.Collections.Generic;
using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class RandomDoorSpawnpoint : RandomSpawnpoint
    {
        private IEnumerable<DoorType> _doors;

        public RandomDoorSpawnpoint(IEnumerable<DoorType> doors) => _doors = doors;

        public RandomDoorSpawnpoint(params DoorType[] doors) => _doors = doors;

        public override Vector3 SelectRandom()
        {
            var type = _doors.GetRandomValue();

            var door = Door.Get(door => door.Type == type).GetRandomValue();

            door.IsOpen = true;

            return door.Position + Vector3.up;
        }
    }
}
