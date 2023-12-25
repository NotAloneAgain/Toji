using Exiled.API.Extensions;
using Exiled.API.Features;
using Toji.Teslas.API;
using UnityEngine;

namespace Toji.Teslas.Handlers
{
    internal sealed class MapHandlers
    {
        public void OnGenerated()
        {
            while (TeslaGateController.Singleton.TeslaGates.Count > 3)
            {
                var tesla = TeslaGateController.Singleton.TeslaGates.GetRandomValue();

                if (tesla == null)
                {
                    continue;
                }

                try
                {
                    tesla.SpawnWorkStation(Vector3.back * 2.16f + Vector3.right * 1.14f);
                    tesla.SpawnWorkStation(Vector3.forward * 2.16f + Vector3.left * 1.14f, Quaternion.Euler(0, 180, 0));
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex);
                }

                tesla.Delete();
            }
        }
    }
}
