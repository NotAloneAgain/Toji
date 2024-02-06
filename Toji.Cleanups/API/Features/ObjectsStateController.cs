using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Mirror;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;
using Toji.Global;
using Exiled.API.Features.Roles;

namespace Toji.Cleanups.API.Features
{
    public static class ObjectsStateController
    {
        private static Dictionary<Player, List<Ragdoll>> _unspawnedRagdolls;
        private static Dictionary<Player, List<Pickup>> _unspawnedPickups;
        private static HashSet<ItemType> _whitelist;

        static ObjectsStateController()
        {
            _unspawnedRagdolls = new Dictionary<Player, List<Ragdoll>>(Server.MaxPlayerCount);
            _unspawnedPickups = new Dictionary<Player, List<Pickup>>(Server.MaxPlayerCount);
            _whitelist = [ItemType.MicroHID, ItemType.SCP2176, ItemType.SCP018, ItemType.ParticleDisruptor, ItemType.Jailbird, ItemType.KeycardO5];
        }

        public static void Reset() => _unspawnedPickups.Clear();

        public static bool TryChangeState(List<Player> players, Pickup pickup)
        {
            if (pickup.InUse || !pickup.IsSpawned || pickup.IsLocked || pickup.IsInLocker())
            {
                return false;
            }

            var zone = (pickup.Room?.Zone ?? ZoneType.Unspecified);

            var maxDistance = zone == ZoneType.Surface ? 60 : 30;

            foreach (var player in players)
            {
                if (!Check(player, pickup, zone, maxDistance))
                {
                    if (_unspawnedPickups.TryGetValue(player, out var value) && value.Contains(pickup))
                    {
                        player.SpawnPersonally(pickup.GameObject);

                        value.Remove(pickup);
                    }

                    continue;
                }

                _unspawnedPickups.SetOrAddArr(player, pickup);

                player.UnSpawnPersonally(pickup.GameObject);
            }

            return true;
        }

        public static bool TryChangeState(List<Player> players, Ragdoll ragdoll)
        {
            var zone = ragdoll.Room?.Zone ?? ZoneType.Unspecified;

            var maxDistance = zone == ZoneType.Surface ? 60 : 30;

            foreach (var player in players)
            {
                if (!Check(player, ragdoll, zone, maxDistance))
                {
                    if (_unspawnedRagdolls.TryGetValue(player, out var value) && value.Contains(ragdoll))
                    {
                        player.SpawnPersonally(ragdoll.GameObject);

                        value.Remove(ragdoll);
                    }

                    continue;
                }

                _unspawnedRagdolls.SetOrAddArr(player, ragdoll);

                player.UnSpawnPersonally(ragdoll.GameObject);
            }

            return true;
        }

        private static bool Check(Player player, Pickup pickup, ZoneType zone, int maxDistance)
        {
            if (player.Role.Is(out Scp079Role role))
            {
                return pickup.Type is ItemType.SCP244a or ItemType.SCP244b || !_whitelist.Contains(pickup.Type) && (role.Camera.Zone != zone || Vector3.Distance(role.Camera.Position, pickup.Position) > maxDistance);
            }

            if (player.IsDead)
            {
                return _whitelist.Contains(pickup.Type);
            }

            return Vector3.Distance(player.Position, pickup.Position) > maxDistance;
        }

        private static bool Check(Player player, Ragdoll ragdoll, ZoneType zone, int maxDistance)
        {
            if (player.Role.Is(out Scp079Role role))
            {
                return role.Camera.Zone != zone || Vector3.Distance(role.Camera.Position, ragdoll.Position) > maxDistance;
            }

            if (player.IsDead)
            {
                return true;
            }

            return Vector3.Distance(player.Position, ragdoll.Position) > maxDistance;
        }

        private static void SpawnPersonally(this Player player, GameObject obj)
        {
            if (!obj.TryGetComponent<NetworkIdentity>(out var identity))
            {
                return;
            }

            NetworkServer.ShowForConnection(identity, player.Connection);
        }

        private static void UnSpawnPersonally(this Player player, GameObject obj)
        {
            if (!obj.TryGetComponent<NetworkIdentity>(out var identity))
            {
                return;
            }

            NetworkServer.HideForConnection(identity, player.Connection);
        }
    }
}