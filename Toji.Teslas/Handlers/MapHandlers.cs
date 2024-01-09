using Exiled.API.Extensions;
using Exiled.API.Features;
using System.Collections.Generic;
using Toji.Teslas.API;
using UnityEngine;

namespace Toji.Teslas.Handlers
{
    internal sealed class MapHandlers
    {
        private List<float> _triggerRanges;
        private List<float> _idleRanges;
        private List<float> _cooldowns;

        internal MapHandlers()
        {
            _triggerRanges = [4.8f, 4.9f, 5, 5.1f, 5.21f, 5.3f];
            _idleRanges = [6.55f, 6.61f, 6.49f, 6.4f, 6.33f, 6.31f, 6.28f, 6.14f, 6, 5.98f, 5.9f, 5.8f];
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
                tesla.TriggerRange = _cooldowns.RandomItem();
                tesla.IdleRange = _cooldowns.RandomItem();
            }
        }
    }
}
