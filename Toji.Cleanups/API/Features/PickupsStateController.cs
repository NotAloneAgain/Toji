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
    public static class PickupsStateController
    {
        private static Dictionary<Player, List<Pickup>> _unspawned;
        private static HashSet<ItemType> _whitelist;

        static PickupsStateController()
        {
            _unspawned = new Dictionary<Player, List<Pickup>>(Server.MaxPlayerCount);
            _whitelist = [ItemType.MicroHID, ItemType.SCP2176, ItemType.SCP018, ItemType.ParticleDisruptor, ItemType.Jailbird];
        }

        public static void Reset() => _unspawned.Clear();

        public static bool TryChangeState(List<Player> players, Pickup pickup)
        {
            var zone = (pickup.Room?.Zone ?? ZoneType.Unspecified);

            var maxDistance = zone == ZoneType.Surface ? 60 : 30;

            foreach (var player in players)
            {
                if (!Check(player, pickup, zone, maxDistance))
                {
                    if (_unspawned.TryGetValue(player, out var value) && value.Contains(pickup))
                    {
                        player.SpawnPersonally(pickup);

                        value.Remove(pickup);
                    }

                    continue;
                }

                _unspawned.SetOrAddArr(player, pickup);

                player.UnSpawnPersonally(pickup);
            }

            return true;
        }

        private static bool Check(Player player, Pickup pickup, ZoneType zone, int maxDistance)
        {
            if (player.Role.Is(out Scp079Role role))
            {
                return pickup.Type is ItemType.SCP244a or ItemType.SCP244b || role.Camera.Zone != zone || !_whitelist.Contains(pickup.Type) && role.Camera.Room != pickup.Room;
            }

            if (Vector3.Distance(player.Position, pickup.Position) > maxDistance)
            {
                return true;
            }

            return false;
        }

        private static void SpawnPersonally(this Player player, Pickup pickup)
        {
            if (!pickup.GameObject.TryGetComponent<NetworkIdentity>(out var identity))
            {
                return;
            }

            NetworkServer.ShowForConnection(identity, player.Connection);
        }

        private static void UnSpawnPersonally(this Player player, Pickup pickup)
        {
            if (!pickup.GameObject.TryGetComponent<NetworkIdentity>(out var identity))
            {
                return;
            }

            NetworkServer.HideForConnection(identity, player.Connection);
        }
    }
}