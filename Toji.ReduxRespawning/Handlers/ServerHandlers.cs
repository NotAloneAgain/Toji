using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using Respawning;
using System.Collections.Generic;
using System.Linq;
using Toji.ReduxRespawning.API;
using UnityEngine;

namespace Toji.ReduxRespawning.Handlers
{
    internal sealed class ServerHandlers
    {
        public void OnSelectingTeam(SelectingRespawnTeamEventArgs ev)
        {
            var players = ListPool<Player>.Pool.Get(4);

            var spectators = Player.List.Where(ply => ply != null && !ply.IsHost && ply.Role.Type == RoleTypeId.Spectator).ToList();

            if (spectators.Count < 14)
            {
                ListPool<Player>.Pool.Return(players);

                return;
            }

            spectators.Sort((a, b) => (b.Role as SpectatorRole).DeadTime.CompareTo((a.Role as SpectatorRole).DeadTime));

            for (int i = 0; i < 4 && i < spectators.Count; i++)
            {
                players.Add(spectators[i]);
            }

            ev.Team.SpawnSquad(players);

            ListPool<Player>.Pool.Return(players);
        }

        public void OnRoundStarted()
        {
            if (Random.Range(0, 100) <= 71)
            {
                Extensions.ReplaceToChaos = false;

                return;
            }

            Extensions.ReplaceToChaos = true;

            Timing.RunCoroutine(_Respawn());
        }

        private IEnumerator<float> _Respawn()
        {
            yield return Timing.WaitForSeconds(12);

            var players = Extensions.WaitingRespawn.ToList();

            if (!players.Any())
            {
                yield break;
            }

            foreach (var ply in players)
            {
                ply.RemoveFromWaiting();
            }

            SpawnableTeamType.ChaosInsurgency.SpawnSquad(players);
        }
    }
}
