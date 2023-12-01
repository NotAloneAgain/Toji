using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using System.Collections.Generic;
using Toji.Classes.API.Extensions;
using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class RandomRoomSpawnpoint : RandomSpawnpoint
    {
        private IEnumerable<RoomType> _rooms;

        public RandomRoomSpawnpoint(IEnumerable<RoomType> rooms) => _rooms = rooms;

        public RandomRoomSpawnpoint(params RoomType[] rooms) => _rooms = rooms;

        public override Vector3 SelectRandom()
        {
            var type = _rooms.GetRandomValue();

            return Room.Get(room => room.Type == type).GetRandomValue().GetSafePosition() + Vector3.up * 2;
        }
    }
}
