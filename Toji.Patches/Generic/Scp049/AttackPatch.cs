using HarmonyLib;
using Mirror;
using PlayerRoles.PlayableScps.Scp049;
using PlayerStatsSystem;
using Utils.Networking;

namespace Toji.Patches.Generic.Scp049
{
    [HarmonyPatch(typeof(Scp049AttackAbility), nameof(Scp049AttackAbility.ServerProcessCmd))]
    internal static class AttackPatch
    {
        private static bool Prefix(Scp049AttackAbility __instance, NetworkReader reader)
        {
            if (!__instance.Cooldown.IsReady || __instance._resurrect.IsInProgress)
                return false;

            __instance._target = reader.ReadReferenceHub();

            if (__instance._target == null || !__instance.IsTargetValid(__instance._target))
                return false;

            __instance.Cooldown.Trigger(1.4);

            __instance._target.playerStats.DealDamage(new Scp049DamageHandler(__instance.Owner, -1f, 0));

            __instance.ServerSendRpc(true);

            __instance.GetSubroutine<Scp049SenseAbility>(out var sense);

            sense?.OnServerHit(__instance._target);

            Hitmarker.SendHitmarkerDirectly(__instance.Owner, 1);

            return false;
        }
    }
}
