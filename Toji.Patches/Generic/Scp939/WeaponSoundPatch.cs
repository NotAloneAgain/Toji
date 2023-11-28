using CustomPlayerEffects;
using HarmonyLib;
using InventorySystem.Items.Firearms;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp939.Ripples;
using PluginAPI.Core;
using RelativePositioning;
using System;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Characteristics;
using Toji.NwPluginAPI.API.Extensions;
using UnityEngine;

#pragma warning disable IDE0060

namespace Toji.Patches.Generic.Scp939
{
    [HarmonyPatch(typeof(FirearmRippleTrigger), nameof(FirearmRippleTrigger.OnServerSoundPlayed))]
    internal static class WeaponSoundPatch
    {
        private static bool Prefix(FirearmRippleTrigger __instance, Firearm firearm, byte audioId, float dis)
        {
            try
            {
                if (__instance == null || firearm == null || !NetworkServer.active)
                {
                    return false;
                }

                if (firearm.Owner == null || firearm.Owner.gameObject == null || !Player.TryGet(firearm.Owner.gameObject, out Player player))
                {
                    return false;
                }

                if (player.Team is Team.Dead or Team.OtherAlive or Team.SCPs)
                {
                    return false;
                }

                var role = player.RoleBase as HumanRole;

                if (player.HasEffect<Invisible>() || role == null)
                {
                    return false;
                }

                Vector3 pos = __instance.CastRole.FpcModule.Position;
                Vector3 humanPos = role.FpcModule.Position;

                if ((pos - humanPos).sqrMagnitude > dis * dis || __instance.CheckVisibility(player.ReferenceHub) || player.Role == RoleTypeId.Tutorial)
                {
                    return false;
                }

                if (player.TryGetSubclass(out var subclass) && subclass.Characteristics.Find(x => x is SoundCharacteristics) is SoundCharacteristics sound && sound.Value)
                {
                    return false;
                }

                __instance._syncRipplePos = new RelativePosition(role.FpcModule.Position);
                __instance._syncRoleColor = role.RoleTypeId;
                __instance._syncPlayer = firearm.Owner;
                __instance.ServerSendRpcToObservers();

                return false;
            }
            catch (Exception err)
            {
                Log.Error($"[FootstepSoundPatch] Error occured: {err}");

                return true;
            }
        }
    }
}
