using GameCore;
using HarmonyLib;
using Hazards;
using MapGeneration;
using MEC;
using Mirror;
using PlayerRoles;
using PluginAPI.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Printer = PluginAPI.Core.Log;

namespace Toji.Patches.Generic.Sinkhole
{
    [HarmonyPatch(typeof(SinkholeEnvironmentalHazard), nameof(SinkholeEnvironmentalHazard.Start))]
    internal static class SpawnPatch
    {
        private static HashSet<Vector3> _positions = new HashSet<Vector3>();
        private static int _spawned = 0;

        private static bool Prefix(SinkholeEnvironmentalHazard __instance)
        {
            if (!NetworkServer.active)
            {
                return false;
            }

            Timing.RunCoroutine(TrySpawn(__instance));

            return false;
        }

        private static IEnumerator<float> TrySpawn(SinkholeEnvironmentalHazard sinkhole)
        {
            yield return Timing.WaitUntilTrue(() => RoundStart.singleton != null && RoundStart.singleton.Timer == -1);

            yield return Timing.WaitForSeconds(5);

            var has106 = Player.GetPlayers().Any(x => x.Role == RoleTypeId.Scp106);
            var chance = has106 switch
            {
                true => _spawned switch
                {
                    0 => 101,
                    1 => 80,
                    2 => 30,
                    _ => 0
                },
                false => _spawned switch
                {
                    0 => 101,
                    1 => 101,
                    _ => 0
                },
            };

            if (chance < Random.Range(1, 101))
            {
                yield break;
            }

            NetworkServer.Spawn(sinkhole.gameObject);

            var position = sinkhole.transform.position;

            if (sinkhole.netId == 0U || _positions.Any(pos => Vector3.Distance(pos, position) < 2))
            {
                NetworkServer.Destroy(sinkhole.gameObject);

                yield break;
            }

            if (_spawned == 1)
            {
                var rooms = RoomIdentifier.AllRoomIdentifiers.Where(room => room.Zone == FacilityZone.Entrance && room.name.IndexOf("EZ_Crossing", System.StringComparison.OrdinalIgnoreCase) >= 0);

                var room = rooms.ElementAt(Random.Range(0, rooms.Count()));

                NetworkServer.UnSpawn(sinkhole.gameObject);

                var result = room.transform.position - Vector3.up * 0.12f;

                sinkhole.transform.position = result;
                position = result;

                sinkhole.MaxDistance *= 0.6f;
                sinkhole.transform.localScale *= 0.7f;

                NetworkServer.Spawn(sinkhole.gameObject);
            }

            _spawned++;
            _positions.Add(position);

            Console.AddDebugLog("MAPGEN", "Spawning hazard: \"" + sinkhole.gameObject.name + "\"", MessageImportance.LessImportant, true);

            Printer.Info($"Spawning hazard: \"{sinkhole.gameObject.name}\", round is{(RoundStart.RoundStarted ? " started." : "n't started.")}");
        }

        public static void Reset()
        {
            _spawned = 0;
            _positions.Clear();
        }
    }
}
