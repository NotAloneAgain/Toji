using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Server;
using GameCore;
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

            var spectators = ListPool<Player>.Pool.Get(Player.List.Where(ply => ply != null && !ply.IsHost && ply.Role.Type == RoleTypeId.Spectator));

            if (spectators.Count < 14)
            {
                ListPool<Player>.Pool.Return(players);
                ListPool<Player>.Pool.Return(spectators);

                return;
            }

            spectators.Sort((a, b) => (b.Role as SpectatorRole).DeadTime.CompareTo((a.Role as SpectatorRole).DeadTime));

            int max = Mathf.Clamp(Mathf.RoundToInt(spectators.Count * 0.28f), 4, 8);

            for (int i = 0; i < max && i < spectators.Count; i++)
            {
                players.Add(spectators[i]);
            }

            ListPool<Player>.Pool.Return(spectators);

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

            Extensions.ReplaceQueue = Extensions.GetRolesQueue(SpawnableTeamType.ChaosInsurgency, GetMaxGuardsCount());
        }

        private int GetMaxGuardsCount()
        {
            int max = Player.List.Count + 5;

            var data = ConfigFile.ServerConfig.GetString("team_respawn_queue", "4014314031441404134041434414");

            int count = 0;

            for (int index = 0; index < max && index < data.Length; index++)
            {
                var value = data[index];

                if (value != '1')
                {
                    continue;
                }

                count++;
            }

            return count;
        }
    }
}
