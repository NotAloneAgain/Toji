using MapGeneration;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toji.Teslas.API
{
    public static class Extensions
    {
        private static List<Vector3> _deletedTeslas;

        static Extensions() => _deletedTeslas = new(10);

        public static void Delete(this TeslaGate gate)
        {
            _deletedTeslas.Add(gate.transform.position);

            TeslaGateController.Singleton.TeslaGates.Remove(gate);

            NetworkServer.Destroy(gate.gameObject);
        }

        public static bool IsDeleted(this TeslaGate gate) => _deletedTeslas.Any(x => RoomIdUtils.IsTheSameRoom(x, gate.transform.position));
    }
}
