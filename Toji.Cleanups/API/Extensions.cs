using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toji.Cleanups.API
{
    public static class Extensions
    {
        private static Dictionary<ZoneType, Dictionary<ItemType, int>> _zonesLimits;
        private static Dictionary<ItemType, int> _globalLimits;

        static Extensions()
        {
            _zonesLimits = new();
            _globalLimits = new();
        }

        public static Bounds FindBounds(this GameObject gameObject)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                return renderer.bounds;
            }
            else
            {
                var bounds = new Bounds(Vector3.zero, Vector3.zero);

                Renderer[] childRenderers = gameObject.GetComponentsInChildren<Renderer>();

                foreach (Renderer childRenderer in childRenderers)
                {
                    bounds.Encapsulate(childRenderer.bounds);
                }

                return bounds;
            }
        }

        public static bool IsInLocker(this IPosition position) => position.Position.IsInLocker();

        public static bool IsInLocker(this Vector3 pos)
        {
            foreach (var locker in Map.Lockers)
            {
                if (locker == null || locker.Loot == null || !locker.Loot.Any() || locker.Chambers == null || !locker.Chambers.Any())
                {
                    continue;
                }

                foreach (var chamber in locker.Chambers)
                {
                    if (chamber == null || chamber._wasEverOpened)
                    {
                        continue;
                    }

                    var bounds = chamber.gameObject.FindBounds();

                    if (bounds.Contains(pos))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsValid(this GameObject gameObject)
        {
            var room = Room.FindParentRoom(gameObject);

            if (room == null)
            {
                return false;
            }

            var zone = room.Zone;

            if (zone != ZoneType.Surface && Warhead.IsDetonated || Map.IsLczDecontaminated && zone == ZoneType.LightContainment)
            {
                return false;
            }

            return true;
        }

        public static bool IsInLcz(this GameObject gameObject) => (Room.FindParentRoom(gameObject)?.Zone ?? ZoneType.Unspecified) == ZoneType.LightContainment;

        public static bool IsInComplex(this GameObject gameObject) => (Room.FindParentRoom(gameObject)?.Zone ?? ZoneType.Unspecified) == ZoneType.Surface;
    }
}
