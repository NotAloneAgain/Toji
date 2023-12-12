using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using System.Collections.Generic;
using Toji.Cleanups.API.Interfaces;

namespace Toji.Cleanups.API.Features
{
    public abstract class ItemCleanup : BaseCleanup, IItemCleanup
    {
        public abstract void Cleanup(List<Pickup> pickups, List<Player> players, out float cooldown);
    }
}
