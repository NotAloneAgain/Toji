using Exiled.API.Extensions;
using Exiled.API.Features;
using InventorySystem.Items.Firearms.Attachments;
using Mirror;
using System.Linq;
using Toji.Teslas.API;
using UnityEngine;

namespace Toji.Teslas.Handlers
{
    internal sealed class MapHandlers
    {
        private WorkstationController _workstationPrefab;

        public WorkstationController Prefab
        {
            get
            {
                if (_workstationPrefab == null)
                {
                    var dict = NetworkClient.prefabs.ToDictionary(key => key.Key.ToString(), value => value.Value);

                    if (dict.TryGetValue("1783091262", out var value) && value.TryGetComponent<WorkstationController>(out var station))
                    {
                        _workstationPrefab = station;
                    }
                }

                return _workstationPrefab;
            }
        }

        public void OnGenerated()
        {
            var count = TeslaGateController.Singleton.TeslaGates.Count;

            if (count > 2)
            {
                int offset = 1;

                do
                {
                    var tesla = TeslaGateController.Singleton.TeslaGates.GetRandomValue();

                    if (tesla == null)
                    {
                        break;
                    }

                    var pos = tesla.Position;
                    var rot = tesla.Room.transform.rotation;

                    Log.Info(rot);

                    try
                    {
                        var back = Vector3.back * 2;
                        var forward = Vector3.forward * 2;
                        var invertedRotation = Quaternion.Euler(Vector3.up * 180);

                        NetworkServer.Spawn(Object.Instantiate(Prefab, pos + rot * back, rot).gameObject);

                        Vector3 spawnPos = pos + rot * forward;

                        if (Physics.OverlapBox(spawnPos, Vector3.one, invertedRotation).Length > 0 || rot is { y: >= 0.7f, w: >= 0.7f })
                        {
                            invertedRotation = Quaternion.Euler(Vector3.up * -90);
                        }

                        NetworkServer.Spawn(Object.Instantiate(Prefab, spawnPos, invertedRotation).gameObject);

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error(ex);
                    }

                    tesla.Delete();

                    offset++;
                } while (count - offset > 2);
            }
        }
    }
}
