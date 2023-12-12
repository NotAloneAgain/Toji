using Exiled.API.Features;
using System.Collections.Generic;

namespace Toji.Cleanups.API.Interfaces
{
    public interface IRagdollCleanup
    {
        void Cleanup(List<Ragdoll> ragdolls, List<Player> players, out float cooldown);
    }
}
