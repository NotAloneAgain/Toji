using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using UnityEngine;

namespace Toji.BetterWarhead.API
{
    public static class Extensions
    {
        public static void LockdownDoor(this Door door, DoorLockType type = DoorLockType.Warhead)
        {
            door.ChangeLock(type);
            door.IsOpen = true;
        }

        public static bool InComplex(this Vector3 vector) => vector.y >= 980;

        public static Bounds CalculateBounds(params Vector3[] vectors)
        {
            var result = new Bounds();

            Vector3 min = Vector3.zero;
            Vector3 max = Vector3.zero;
            Vector3 center = Vector3.zero;

            foreach (var vector in vectors)
            {
                min.x = Mathf.Min(vector.x, min.x);
                min.y = Mathf.Min(vector.y, min.y);
                min.z = Mathf.Min(vector.z, min.z);

                max.x = Mathf.Max(vector.x, max.x);
                max.y = Mathf.Max(vector.y, max.y);
                max.z = Mathf.Max(vector.z, max.z);

                center += vector;
            }

            center /= vectors.Length;

            result.SetMinMax(min, max);
            result.center = center;

            return result;
        }

        public static Bounds CalculateBounds(this (float Min, float Max) y, params Vector2[] vectors)
        {
            var result = new Bounds();

            Vector3 min = Vector3.up * y.Min;
            Vector3 max = Vector3.up * y.Max;

            foreach (var vector in vectors)
            {
                min.x = Mathf.Min(vector.x, min.x);
                min.z = Mathf.Min(vector.y, min.z);

                max.x = Mathf.Min(vector.x, max.x);
                max.z = Mathf.Min(vector.y, max.z);
            }

            result.SetMinMax(min, max);

            return result;
        }
    }
}
