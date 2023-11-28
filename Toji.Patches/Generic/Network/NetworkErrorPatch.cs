using HarmonyLib;
using LiteNetLib;

namespace Toji.Patches.Generic.Scp049
{
    [HarmonyPatch(typeof(NetDebug), nameof(NetDebug.WriteError))]
    internal static class NetworkErrorPatch
    {
        private static bool Prefix() => false;
    }
}
