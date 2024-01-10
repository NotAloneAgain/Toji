/*using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using System.Collections.Generic;
using UnityEngine;

namespace Toji.Cleanups.API.Features
{
    public static class PickupsStateController
    {
        private static LayerMask _layerMasks;

        static PickupsStateController()
        {
            _layerMasks = ~LayerMask.GetMask("Player", "Pickup", "Hitbox", "DestroyedDoor", "BreakableGlass", "SCP018", "Light", "Grenade", "Ragdoll", "TransparentFX", "IgnoreRaycast", "Water", "UI", "InvisibleCollider", "CCTV", "Icon", "Locker", "Light", "Glass", "Door");
        }

        public static bool TryChangeState(List<Player> players, Pickup pickup)
        {
            var pos = pickup.Position;
            bool isCanVisible = false;

            foreach (var player in players)
            {
                var maxDistance = player.Zone == ZoneType.Surface ? 60 : 30;

                if (Vector3.Distance(player.Position, pos) > maxDistance || !AdvancedCheck(player, pickup))
                {
                    continue;
                }

                isCanVisible = true;

                break;
            }

            bool isSpawned = pickup.IsSpawned;

            if (isCanVisible)
            {
                pickup.Spawn();
            }
            else
            {
                // NetworkServer.DestroyObject(NetworkIdentity identity, DestroyMode mode)

                pickup.UnSpawn();
            }

            return isSpawned != pickup.IsSpawned;
        }

        private static bool AdvancedCheck(Player player, Pickup pickup)
        {
            var position = pickup.Position;

            if (Physics.Linecast(player.Position + Vector3.up, position, out var hit, _layerMasks, QueryTriggerInteraction.Ignore) && hit.transform != pickup.Transform)
            {
                return false;
            }

            Vector3 objectDirection = position - player.CameraTransform.position;
            float angle = Vector3.Angle(player.CameraTransform.forward, objectDirection);

            float fieldOfView = 70;

            if (player.CameraTransform.TryGetComponent<UnityEngine.Camera>(out var camera))
            {
                fieldOfView = camera.fieldOfView;
            }

            if (angle > fieldOfView)
            {
                return false;
            }

            return true;
        }
    }
}*/