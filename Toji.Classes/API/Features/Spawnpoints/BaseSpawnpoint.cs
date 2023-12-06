using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public abstract class BaseSpawnpoint
    {
        public abstract Vector3 Position { get; }

        public virtual string PointType => this is StaticSpawnpoint ? "в заданном месте" : this is RandomSpawnpoint ? "в случайном месте" : "около определенного объекта";
    }
}
