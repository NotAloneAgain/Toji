using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using System.Linq;

namespace Toji.Cleanups.Handlers
{
    internal sealed class WarheadHandlers
    {
        public void OnDetonated()
        {
            var pickups = Pickup.List.Where(pickup => pickup?.Room?.Zone != ZoneType.Surface).ToList();
            var ragdolls = Ragdoll.List.Where(ragdoll => ragdoll?.Room?.Zone != ZoneType.Surface).ToList();

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
