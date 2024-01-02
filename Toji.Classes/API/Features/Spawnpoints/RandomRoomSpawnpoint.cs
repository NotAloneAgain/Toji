using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System.Collections.Generic;
using Toji.Classes.API.Extensions;
using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class RandomRoomSpawnpoint(IEnumerable<RoomType> rooms) : RandomSpawnpoint
    {
        public RandomRoomSpawnpoint(params RoomType[] rooms) : this((IEnumerable<RoomType>)rooms) { }

        public override Vector3 SelectRandom()
        {
            var type = rooms.GetRandomValue();

            return Room.Get(room => room.Type == type).GetRandomValue().GetSafePosition() + Vector3.up * 2;
        }
    }
}
