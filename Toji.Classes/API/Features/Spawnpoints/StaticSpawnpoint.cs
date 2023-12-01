using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public class StaticSpawnpoint : BaseSpawnpoint
    {
        private Vector3 _pos;

        public StaticSpawnpoint(Vector3 pos) : base() => _pos = pos;

        public override Vector3 Position => _pos;
    }
}
