using Exiled.API.Enums;
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
            _layerMasks = ~LayerMask.GetMask("Player", "Pickup", "Hitbox", "DestroyedDoor", "BreakableGlass", "SCP018", "Light", "Grenade", "Ragdoll");
        }

        public static bool TryChangeState(List<Player> players, Pickup pickup)
        {
            var pos = pickup.Position;
            bool isCanVisible = false;

            foreach (var player in players)
            {
                if (player == null || player.IsHost || player.IsNPC || player.IsDead)
                {
                    continue;
                }

                var maxDistance = player.Zone == ZoneType.Surface ? 60 : 30;

                if (Vector3.Distance(player.Position, pos) > maxDistance || Physics.Linecast(player.CameraTransform.position, pos, _layerMasks, QueryTriggerInteraction.Ignore))
                {
                    continue;
                }

                Vector3 objectDirection = pos - player.CameraTransform.position;
                float angle = Vector3.Angle(player.CameraTransform.forward, objectDirection);

                float fieldOfView = 60;

                if (player.CameraTransform.TryGetComponent<UnityEngine.Camera>(out var camera))
                {
                    fieldOfView = camera.fieldOfView;
                }

                if (angle > fieldOfView)
                {
                    continue;
                }

                isCanVisible = true;
            }

            bool isSpawned = pickup.IsSpawned;

            if (isCanVisible)
            {
                pickup.Spawn();
            }
            else
            {
                pickup.UnSpawn();
            }

            return isSpawned != pickup.IsSpawned;
        }
    }
}
