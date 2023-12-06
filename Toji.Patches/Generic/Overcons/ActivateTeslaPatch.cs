using HarmonyLib;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp079.GUI;
using PlayerRoles.PlayableScps.Scp079.Overcons;
using Toji.Teslas.API;
using Utils.NonAllocLINQ;

namespace Toji.Patches.Generic.Scp049
{
    [HarmonyPatch(typeof(Scp079TeslaAbility), nameof(Scp079TeslaAbility.IsVisible), MethodType.Getter)]
    internal static class ActivateTeslaPatch
    {
        private static bool Prefix(Scp079TeslaAbility __instance, out bool __result)
        {
            __result = false;

            if (!Scp079CursorManager.LockCameras)
            {
                TeslaOvercon teslaOvercon = OverconManager.Singleton.HighlightedOvercon as TeslaOvercon;

                var cam = __instance.CurrentCamSync.CurrentCamera;

                __result = teslaOvercon != null && TeslaGateController.Singleton.TeslaGates.TryGetFirst(x => RoomIdUtils.IsTheSameRoom(cam.Position, x.transform.position), out var gate) && !gate.IsDeleted();
            }

            return false;
        }
    }
}
