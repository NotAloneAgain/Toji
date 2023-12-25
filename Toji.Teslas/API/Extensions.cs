using Exiled.API.Features;
using InventorySystem.Items.Firearms.Attachments;
using MapGeneration;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toji.Teslas.API
{
    public static class Extensions
    {
        private static WorkstationController _workstationPrefab;
        private static List<Vector3> _deletedTeslas;

        static Extensions() => _deletedTeslas = new(10);

        public static WorkstationController Prefab
        {
            get
            {
                if (_workstationPrefab == null)
                {
                    var dict = NetworkClient.prefabs.ToDictionary(key => key.Key.ToString(), value => value.Value);

                    if (dict.Values.FirstOrDefault(value => value.TryGetComponent<WorkstationController>(out _))?.TryGetComponent<WorkstationController>(out var station) ?? false)
                    {
                        _workstationPrefab = station;
                    }
                }

                return _workstationPrefab;
            }
        }

        public static void Delete(this TeslaGate gate)
        {
            _deletedTeslas.Add(gate.transform.position);

            TeslaGateController.Singleton.TeslaGates.Remove(gate);

            NetworkServer.Destroy(gate.gameObject);
        }

        public static void SpawnWorkStation(this TeslaGate tesla, Vector3 offset, Quaternion? quaternion = null)
        {
            if (Prefab == null)
            {
                Log.Warn("Prefab is null!");

                return;
            }

            var transform = tesla.Room.transform;
            var pos = transform.position;
            var rot = transform.rotation;

            var gameObject = Object.Instantiate(Prefab, pos, rot).gameObject;

            gameObject.transform.SetParent(transform, false);

            gameObject.transform.localPosition = offset;

            if (quaternion != null && quaternion.HasValue)
            {
                gameObject.transform.localRotation = quaternion.Value;
            }
            else
            {
                gameObject.transform.localRotation = tesla.transform.localRotation * Quaternion.Euler(0, 270, 0);
            }

            NetworkServer.Spawn(gameObject);
        }

        public static bool IsDeleted(this TeslaGate gate) => _deletedTeslas.Any(x => RoomIdUtils.IsTheSameRoom(x, gate.transform.position));
    }
}
