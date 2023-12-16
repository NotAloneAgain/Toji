using Exiled.API.Features;
using Mirror;
using PlayerRoles.PlayableScps.Scp049;
using PlayerStatsSystem;
using System;
using Utils.Networking;

namespace Toji.Patches.Generic.Scp049
{
    //[HarmonyPatch(typeof(Scp049AttackAbility), nameof(Scp049AttackAbility.ServerProcessCmd))]
    internal static class DoctorAttackPatch
    {
        private static bool Prefix(Scp049AttackAbility __instance, NetworkReader reader)
        {
            try
            {
                if (!__instance.Cooldown.IsReady || __instance._resurrect.IsInProgress)
                    return false;

                __instance._target = reader.ReadReferenceHub();

                if (__instance._target == null || !__instance.IsTargetValid(__instance._target))
                    return false;

                __instance.Cooldown.Trigger(1.4f);

                __instance._target.playerStats.DealDamage(new Scp049DamageHandler(__instance.Owner, -1f, 0));

                __instance.ServerSendRpc(true);

                __instance.GetSubroutine<Scp049SenseAbility>(out var sense);

                sense?.OnServerHit(__instance._target);

                Hitmarker.SendHitmarkerDirectly(__instance.Owner, 1);

                return false;
            }
            catch (Exception err)
            {
                Log.Error($"[DoctorAttackPatch] Error occured: {err}");

                return true;
            }
        }
    }
}
