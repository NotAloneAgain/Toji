using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class StaticSpawnpoint(Vector3 pos) : BaseSpawnpoint()
    {
        public override Vector3 Position => pos;
    }
}
