using Exiled.API.Features;
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
            cooldown = 26;

            foreach (Ragdoll ragdoll in ragdolls)
            {
                if (ragdoll == null)
                {
                    continue;
                }

                ragdoll.Destroy();
            }
        }
    }
}
