using HarmonyLib;
using PluginAPI.Core;
using Toji.Patches.API.Extensions;

namespace Toji.Patches.Generic.Admins
{
    [HarmonyPatch(typeof(ServerRoles), nameof(ServerRoles.TryHideTag))]
    internal static class HideTagPatch
    {
        private static bool Prefix(ServerRoles __instance, ref bool __result)
        {
            if (!Player.TryGet(__instance._hub, out var player))
            {
                return true;
            }

            if (player.IsDonator(out _) || __instance.Group.KickPower <= 0 || __instance.Group.GetNameByGroup() == "mod1")
            {
                __result = false;

                return false;
            }

            return true;
        }
    }
}
