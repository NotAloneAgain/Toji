using Exiled.API.Features;
using HarmonyLib;
using Mirror;
using PlayerRoles.PlayableScps.Scp049;
using PlayerStatsSystem;
using System;
using Toji.Classes.API.Extensions;
using Toji.Classes.Subclasses.Characteristics;
using Toji.Classes.Subclasses.Scp049;
using Utils.Networking;

namespace Toji.Patches.Generic.Scp049
{
    [HarmonyPatch(typeof(Scp049AttackAbility), nameof(Scp049AttackAbility.ServerProcessCmd))]
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

                var cooldown = 1.4;

                if (Player.TryGet(__instance.Owner, out var player) && player.TryGetSubclass(out var subclass) && subclass.Characteristics.Find(x => x is AttackCooldownCharacteristic) is AttackCooldownCharacteristic characteristic)
                {
                    cooldown = characteristic.Value;
                }

                __instance.Cooldown.Trigger(cooldown);

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
