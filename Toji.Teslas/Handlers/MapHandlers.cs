using Exiled.API.Extensions;
using Exiled.API.Features;
using System.Collections.Generic;
using Toji.Teslas.API;
using UnityEngine;

namespace Toji.Teslas.Handlers
{
    internal sealed class MapHandlers
    {
        private List<float> _cooldowns;

        internal MapHandlers()
        {
            _cooldowns = [0.96f, 1, 1.07f, 1.15f, 1.23f, 1.31f, 1.38f, 1.44f, 1.5f, 1.61f, 1.69f, 1.73f, 1.78f, 1.82f, 1.88f, 1.9f, 2, 2.24f];
        }

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

            foreach (var tesla in Exiled.API.Features.TeslaGate.List)
            {
                if (tesla == null || tesla.Base == null || tesla.Base.IsDeleted())
                {
                    continue;
                }

                tesla.CooldownTime = _cooldowns.RandomItem();
            }
        }
    }
}
