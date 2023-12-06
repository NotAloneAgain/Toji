using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public abstract class RandomSpawnpoint : BaseSpawnpoint
    {
        public sealed override Vector3 Position => SelectRandom();

        public abstract Vector3 SelectRandom();
    }
}
