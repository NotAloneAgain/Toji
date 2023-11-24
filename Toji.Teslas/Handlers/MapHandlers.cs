using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Scp079;
using InventorySystem.Items.Firearms.Attachments;
using Mirror;
using PlayerRoles.PlayableScps.Scp079.Overcons;
using System.Linq;
using System.Reflection;
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
                int offset = 0;

                while (count - offset > 2)
                {
                    var tesla = TeslaGateController.Singleton.TeslaGates.GetRandomValue();

                    if (tesla == null)
                    {
                        break;
                    }

                    var pos = tesla.Position;
                    var rot = tesla.Room.transform.rotation;

                    try
                    {
                        var back = Vector3.back * 2;
                        var forward = Vector3.forward * 2;
                        var invertedRotation = Quaternion.Euler(Vector3.up * 180);

                        NetworkServer.Spawn(Object.Instantiate(Prefab, pos + rot * back, rot).gameObject);

                        Vector3 spawnPos = pos + rot * forward;

                        if (Physics.OverlapBox(spawnPos, Vector3.one, invertedRotation).Length > 0)
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
                }
            }
        }
    }
}
