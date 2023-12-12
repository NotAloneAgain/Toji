using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using System.Collections.Generic;

namespace Toji.Cleanups.API.Interfaces
{
    public interface IItemCleanup
    {
        void Cleanup(List<Pickup> pickups, List<Player> players, out float cooldown);
    }
}
