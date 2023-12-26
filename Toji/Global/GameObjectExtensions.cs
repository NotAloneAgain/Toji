using UnityEngine;

namespace Toji.Global
{
    public static class GameObjectExtensions
    {
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
    }
}
