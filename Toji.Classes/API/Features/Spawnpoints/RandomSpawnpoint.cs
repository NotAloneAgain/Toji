using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Toji.Classes.API.Features.Spawnpoints
{
    public abstract class RandomSpawnpoint : BaseSpawnpoint
    {
        public sealed override Vector3 Position => SelectRandom();

        public abstract Vector3 SelectRandom();
    }
}
