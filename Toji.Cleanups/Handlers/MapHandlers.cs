using Exiled.API.Extensions;
using Exiled.API.Features.Pickups;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using System.Linq;
using Exiled.API.Enums;

#pragma warning disable IDE0060

namespace Toji.Cleanups.Handlers
{
    internal sealed class MapHandlers
    {
        public void OnPlacingBulletHole(PlacingBulletHoleEventArgs ev) => ev.IsAllowed = false;

        public void OnSpawningItem(SpawningItemEventArgs ev) => ev.IsAllowed = !ev.Pickup.Type.IsAmmo();

        public void OnDecontaminating(DecontaminatingEventArgs ev)
        {
            var pickups = Pickup.List.Where(pickup => pickup != null && (pickup.Room?.Zone ?? ZoneType.LightContainment) == ZoneType.LightContainment).ToList();
            var ragdolls = Ragdoll.List.Where(ragdoll => ragdoll is not null and { GameObject: not null } && (ragdoll.Room?.Zone ?? ZoneType.LightContainment) == ZoneType.LightContainment).ToList();

            foreach (var pickup in pickups)
            {
                pickup.Destroy();
            }

            foreach (var ragdoll in ragdolls)
            {
                ragdoll.Destroy();
            }
        }
    }
}
