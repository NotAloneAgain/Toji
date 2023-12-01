using CustomPlayerEffects;
using Exiled.API.Features;
using HarmonyLib;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles.PlayableScps.Scp939.Ripples;
using RelativePositioning;
using System;
using Toji.Classes.API.Extensions;
using Toji.Classes.Subclasses.Characteristics;
using Toji.ExiledAPI.Extensions;
using UnityEngine;

namespace Toji.Patches.Generic.Scp939
{
    [HarmonyPatch(typeof(FootstepRippleTrigger), nameof(FootstepRippleTrigger.OnFootstepPlayed))]
    internal static class FootstepSoundPatch
    {
        private static bool Prefix(FootstepRippleTrigger __instance, AnimatedCharacterModel model, float dis)
        {
            try
            {
                if (__instance == null || model == null || !NetworkServer.active)
                {
                    return false;
                }

                if (model.OwnerHub == null || model.OwnerHub.gameObject == null || !Player.TryGet(model.OwnerHub.gameObject, out Player player))
                {
                    return false;
                }

                if (player.Role.Team is Team.Dead or Team.OtherAlive or Team.SCPs)
                {
                    return false;
                }

                var role = player.Role.Base as HumanRole;

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

                if (player.TryGetSubclass(out var subclass) && subclass.Characteristics.Find(x => x is SoundCharacteristic) is SoundCharacteristic sound && sound.Value)
                {
                    return false;
                }

                __instance._syncPlayer = model.OwnerHub;
                __instance._syncPos = new RelativePosition(humanPos);
                __instance._syncDistance = (byte)Mathf.Min(dis, 255);
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
