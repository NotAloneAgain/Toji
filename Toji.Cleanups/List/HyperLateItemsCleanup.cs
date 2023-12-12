using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using System.Collections.Generic;
using Toji.Cleanups.API.Enums;
using Toji.Cleanups.API.Features;

namespace Toji.Cleanups.List
{
    public class HyperLateItemsCleanup : ItemCleanup
    {
        public override CleanupType Type => CleanupType.Items;

        public override GameStage Stage => GameStage.HyperLate;

        public override void Cleanup(List<Pickup> pickups, List<Player> players, out float cooldown)
        {
            cooldown = 42;

            foreach (var item in pickups)
            {
                if (item.Type is ItemType.Jailbird or ItemType.MicroHID)
                {
                    continue;
                }

                item.Destroy();
            }
        }
    }
}
