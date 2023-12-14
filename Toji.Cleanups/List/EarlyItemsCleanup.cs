using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using System.Collections.Generic;
using System.Linq;
using Toji.Cleanups.API;
using Toji.Cleanups.API.Enums;
using Toji.Cleanups.API.Features;
using Toji.Global;

namespace Toji.Cleanups.List
{
    public class EarlyItemsCleanup : ItemCleanup
    {
        private Dictionary<ZoneType, int> _limits;

        public EarlyItemsCleanup() => _limits = new Dictionary<ZoneType, int>(4)
            {
                { ZoneType.LightContainment, 20 },
                { ZoneType.HeavyContainment, 15 },
                { ZoneType.Entrance, 12 },
                { ZoneType.Surface, 6 },
            };

        public override CleanupType Type => CleanupType.Items;

        public override GameStage Stage => GameStage.Early;

        public override void Cleanup(List<Pickup> pickups, List<Player> players, out float cooldown)
        {
            cooldown = 120;

            Dictionary<ZoneType, int> medical = new Dictionary<ZoneType, int>()
            {
                { ZoneType.LightContainment, 0 },
                { ZoneType.HeavyContainment, 0 },
                { ZoneType.Entrance, 0 },
                { ZoneType.Surface, 0 },
            };

            foreach (var item in pickups)
            {
                var category = item.Type.GetCategory();

                if (category == ItemCategory.Ammo)
                {
                    item.Destroy();

                    continue;
                }

                var room = item.Room;

                if (players.Any(ply => ply.CurrentRoom == room && (room.Type != RoomType.Surface || ply.Position.GetDistance(item.Position) <= 20)))
                {
                    continue;
                }

                if (room.Type is RoomType.Lcz914 or RoomType.Hcz079 || item.IsInLocker() || category is ItemCategory.MicroHID or ItemCategory.Armor or ItemCategory.SCPItem or ItemCategory.Keycard || item.Type is ItemType.Jailbird or ItemType.GunFRMG0)
                {
                    continue;
                }

                var zone = room.Zone;

                if (category == ItemCategory.Medical && medical.ContainsKey(zone) && medical[zone] != _limits[zone])
                {
                    medical[zone]++;

                    continue;
                }

                item.Destroy();
            }
        }
    }
}
