using UnityEngine;

namespace Toji.Global
{
    public static class VectorExtensions
    {
        public static float GetDistance(this Vector3 vector, Vector3 other) => Vector3.Distance(vector, other);
    }
}
