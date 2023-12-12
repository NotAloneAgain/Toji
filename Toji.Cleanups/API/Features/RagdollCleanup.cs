using Exiled.API.Features;
using System.Collections.Generic;
using Toji.Cleanups.API.Interfaces;

namespace Toji.Cleanups.API.Features
{
    public abstract class RagdollCleanup : BaseCleanup, IRagdollCleanup
    {
        public abstract void Cleanup(List<Ragdoll> ragdolls, List<Player> players, out float cooldown);
    }
}
