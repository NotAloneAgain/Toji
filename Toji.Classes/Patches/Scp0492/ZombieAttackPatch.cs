using Exiled.API.Features;
using HarmonyLib;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp049.Zombies;
using PlayerRoles.PlayableScps.Subroutines;
using RelativePositioning;
using System;
using System.Collections.Generic;
using Toji.Classes.API.Extensions;
using Toji.Classes.Subclasses.Characteristics;
using UnityEngine;
using Utils.Networking;
using Utils.NonAllocLINQ;

namespace Toji.Classes.Patches.Scp0492
{
    [HarmonyPatch(typeof(ScpAttackAbilityBase<ZombieRole>), nameof(ScpAttackAbilityBase<ZombieRole>.ServerProcessCmd))]
    internal static class ZombieAttackPatch
    {
        private static bool Prefix(ScpAttackAbilityBase<ZombieRole> __instance, NetworkReader reader)
        {
            try
            {
                __instance.ServerProcessCmd(reader);

                RelativePosition relativePosition = RelativePositionSerialization.ReadRelativePosition(reader);

                if (relativePosition.WaypointId == 0)
                {
                    __instance.AttackTriggered = true;
                    __instance.ServerSendRpc(true);

                    return false;
                }

                if (!__instance._serverCooldown.TolerantIsReady && !__instance.Owner.isLocalPlayer)
                {
                    return false;
                }

                __instance.AttackTriggered = false;

                Vector3 position = relativePosition.Position;
                Quaternion value = LowPrecisionQuaternionSerializer.ReadLowPrecisionQuaternion(reader).Value;
                ScpAttackAbilityBase<ZombieRole>.BacktrackedPlayers.Add(new FpcBacktracker(__instance.Owner, position, value));
                List<ReferenceHub> list = new List<ReferenceHub>();

                while (reader.Position < reader.Capacity)
                {
                    ReferenceHub referenceHub = ReferenceHubReaderWriter.ReadReferenceHub(reader);

                    list.Add(referenceHub);

                    RelativePosition relativePosition2 = RelativePositionSerialization.ReadRelativePosition(reader);

                    if (referenceHub != null && referenceHub.roleManager.CurrentRole is HumanRole)
                    {
                        ScpAttackAbilityBase<ZombieRole>.BacktrackedPlayers.Add(new FpcBacktracker(referenceHub, relativePosition2.Position));
                    }
                }

                __instance.ServerPerformAttack();

                HashsetExtensions.ForEach(ScpAttackAbilityBase<ZombieRole>.BacktrackedPlayers, delegate (FpcBacktracker x)
                {
                    x.RestorePosition();
                });

                double cooldown = __instance.BaseCooldown;

                if (Player.TryGet(__instance.Owner, out var player) && player.TryGetSubclass(out var subclass) && subclass.Characteristics.Find(x => x is AttackCooldownCharacteristic) is AttackCooldownCharacteristic characteristic)
                {
                    cooldown = characteristic.Value;
                }

                __instance._serverCooldown.Trigger(cooldown);

                __instance.DetectedPlayers.Clear();
                ScpAttackAbilityBase<ZombieRole>.BacktrackedPlayers.Clear();

                __instance.ServerSendRpc(true);

                return false;
            }
            catch (Exception err)
            {
                Log.Error($"[ZombieAttackPatch] Error occured: {err}");

                return true;
            }
        }
    }
}
