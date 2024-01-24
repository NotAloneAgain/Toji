using Exiled.API.Features;
using Mirror;
using System.Collections.Generic;
using Toji.Cleanups.API.Enums;
using Toji.Cleanups.API.Features;

namespace Toji.Cleanups.List
{
    public class HyperLateRagdollsCleanup : RagdollCleanup
    {
        public override CleanupType Type => CleanupType.Ragdolls;

        public override GameStage Stage => GameStage.HyperLate;

        public override void Cleanup(List<Ragdoll> ragdolls, List<Player> players, out float cooldown)
        {
            cooldown = 42;

            foreach (var ragdoll in ragdolls)
            {
                if (ragdoll == null || ragdoll.ExistenceTime <= 9)
                {
                    continue;
                }

                try
                {
                    NetworkServer.Destroy(ragdoll.GameObject);
                }
                catch { }

                try
                {
                    ragdoll.Destroy();
                }
                catch { }
            }
        }
    }
}
