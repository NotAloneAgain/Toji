using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using UnityEngine;

namespace Toji.Classes.API.Extensions
{
    public static class SpawnpointExtensions
    {
        public static Vector3 GetSafePosition(this Room room)
        {
            if (!room.Type.IsDangerous())
            {
                return room.Position;
            }

            return room.Doors.GetRandomValue().Position;
        }

        public static bool IsDangerous(this RoomType room) => room switch
        {
            RoomType.EzShelter or RoomType.EzCollapsedTunnel or RoomType.HczTestRoom or RoomType.HczTestRoom or RoomType.Lcz173 or RoomType.Lcz330 => true,
            _ => false
        };
    }
}
