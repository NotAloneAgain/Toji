using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using PlayerRoles;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Toji.ReduxRespawning.API
{
    public static class Extensions
    {
        private static IReadOnlyCollection<Vector3> _rotationsPoints;
        private static IReadOnlyList<Vector3> _chaosSpawns;
        private static IReadOnlyList<Vector3> _ntfSpawns;
        private static List<Player> _awaitingRespawn;

        static Extensions()
        {
            _awaitingRespawn = new(20);

            _rotationsPoints = [

                new (0, 991.881f, -37),
                new (127.206f, 988.792f, 20.868f),
            ];

            _chaosSpawns = [

                new (-40.391f, 991.881f, -36.098f),
                new (-39.762f, 991.881f, -36.098f),
                new (-39.351f, 991.881f, -36.098f),
                new (-38.922f, 991.881f, -36.094f),
                new (-37.766f, 991.881f, -36.086f),
                new (-37.238f, 991.881f, -36.094f),
                new (-39.965f, 991.881f, -36.77f),
                new (-39.969f, 991.881f, -35.977f),
                new (-39.764f, 991.881f, -35.969f),
                new (-33.578f, 991.881f, -35.375f)
            ];
            _ntfSpawns = [

                new (128.304f, 988.792f, 28.55f),
                new (128.069f, 988.792f, 27.07f),
                new (128.518f, 988.792f, 26.961f),
                new (128.925f, 988.792f, 27.515f),
                new (127.448f, 988.792f, 27.605f),
                new (127.491f, 988.792f, 27.189f),
                new (127.929f, 988.792f, 26.523f),
                new (128.292f, 988.792f, 26.339f),
                new (128.738f, 988.792f, 27.189f),
                new (131.378f, 988.792f, 27.117f),
                new (124.409f, 988.792f, 26.925f),
                new (128.862f, 988.792f, 24.011f),
                new (132.421f, 988.792f, 20.886f)
            ];
        }

        public static IReadOnlyList<Vector3> ChaosSpawns => _chaosSpawns;

        public static IReadOnlyList<Vector3> NtfSpawns => _ntfSpawns;

        public static bool ReplaceToChaos { get; internal set; }

        public static Queue<RoleTypeId> ReplaceQueue { get; internal set; }

        public static Queue<RoleTypeId> GetRolesQueue(this SpawnableTeamType team, int playerCount)
        {
            var list = team switch
            {
                SpawnableTeamType.ChaosInsurgency => GetChaosRoles(playerCount),
                SpawnableTeamType.NineTailedFox => GetNineTailedFoxRoles(playerCount),
                _ => []
            };

            var queue = QueuePool<RoleTypeId>.Pool.Get(list);

            ListPool<RoleTypeId>.Pool.Return(list);

            return queue;
        }

        private static List<RoleTypeId> GetChaosRoles(int playerCount)
        {
            int chaosRiflemanCount = Math.Max(1, playerCount / 2);
            int chaosMarauderCount = Math.Max(1, playerCount / 4);
            int chaosRepressorCount = chaosMarauderCount;

            if (playerCount >= 3)
            {
                chaosRepressorCount = 1;
            }

            List<RoleTypeId> roles = ListPool<RoleTypeId>.Pool.Get(playerCount);

            for (int i = 0; i < chaosRiflemanCount; i++)
            {
                roles.Add(RoleTypeId.ChaosRifleman);
            }

            for (int i = 0; i < chaosMarauderCount; i++)
            {
                roles.Add(RoleTypeId.ChaosMarauder);
            }

            for (int i = 0; i < chaosRepressorCount; i++)
            {
                roles.Add(RoleTypeId.ChaosRepressor);
            }

            return roles;
        }

        private static List<RoleTypeId> GetNineTailedFoxRoles(int playerCount)
        {
            int ntfSergeantCount = Math.Max(1, playerCount / 4);
            int ntfPrivateCount = Math.Max(3, playerCount - ntfSergeantCount);

            List<RoleTypeId> roles = ListPool<RoleTypeId>.Pool.Get(playerCount);

            for (int i = 0; i < ntfSergeantCount; i++)
            {
                roles.Add(RoleTypeId.NtfSergeant);
            }

            for (int i = 0; i < ntfPrivateCount; i++)
            {
                roles.Add(RoleTypeId.NtfPrivate);
            }

            return roles;
        }

        public static void SpawnSquad(this SpawnableTeamType team, List<Player> players)
        {
            if (team == SpawnableTeamType.None)
            {
                return;
            }

            var roles = team.GetRolesQueue(players.Count);

            if (!roles.Any())
            {
                return;
            }

            (var spawns, var point) = (team == SpawnableTeamType.ChaosInsurgency) switch
            {
                true => (_chaosSpawns, _rotationsPoints.ElementAt(0)),
                false => (_ntfSpawns, _rotationsPoints.ElementAt(1))
            };

            var ordered = players.OrderBy(ply => ply.Role is SpectatorRole spectator ? spectator.DeadTime.TotalSeconds : 0);

            foreach (var player in ordered)
            {
                var role = roles.Dequeue();

                player.Role.Set(role, SpawnReason.Respawn, RoleSpawnFlags.AssignInventory);

                player.Teleport(spawns.GetRandomValue());

                player.Rotation = Quaternion.LookRotation(point);
            }

            QueuePool<RoleTypeId>.Pool.Return(roles);
        }
    }
}
