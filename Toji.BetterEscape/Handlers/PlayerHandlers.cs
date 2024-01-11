using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using Toji.BetterEscape.API;
using Toji.ExiledAPI.Extensions;

namespace Toji.BetterEscape.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (!ev.IsValid() || ev.Player.Role.Team is not Team.FoundationForces and Team.ChaosInsurgency)
            {
                return;
            }

            Timing.RunCoroutine(_CheckEscape(ev.Player, ev.Player.Role.Type));
        }

        private IEnumerator<float> _CheckEscape(Player player, RoleTypeId target)
        {
            while (player?.Role?.Type == target)
            {
                yield return Timing.WaitForSeconds(0.5f);

                if ((player.Position - Escape.WorldPos).sqrMagnitude > Escape.RadiusSqr)
                {
                    continue;
                }

                bool isCuffed = player.IsCuffed;

                if (target != RoleTypeId.FacilityGuard && !isCuffed)
                {
                    continue;
                }

                player.HandleEscape(isCuffed);

                break;
            }
        }
    }
}
