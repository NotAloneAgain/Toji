using CustomPlayerEffects;
using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp939.Ripples;
using PluginAPI.Core;
using System;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Interfaces;
using Toji.NwPluginAPI.API.Extensions;
using UnityEngine;

namespace Toji.Patches.Generic.Scp939
{
    [HarmonyPatch(typeof(DoorRippleTrigger), nameof(DoorRippleTrigger.OnDoorAction))]
    internal static class DoorSoundPatch
    {
        private static bool Prefix(DoorRippleTrigger __instance, DoorVariant dv, DoorAction da, ReferenceHub hub)
        {
            try
            {
                if (__instance == null || da is DoorAction.Opened or DoorAction.Closed || !NetworkServer.active)
                    return false;

                var basicDoor = dv as BasicDoor;

                if (basicDoor == null)
                    return false;

                if (hub == null || hub.gameObject == null || !Player.TryGet(hub.gameObject, out Player player))
                    return false;

                if (player.Team is Team.Dead or Team.OtherAlive or Team.SCPs)
                    return false;

                var role = player.RoleBase as HumanRole;

                if (player.HasEffect<Invisible>() || role == null)
                    return false;

                var magnitude = (dv.transform.position + DoorRippleTrigger.PosOffset - __instance.CastRole.FpcModule.Position).sqrMagnitude;
                var maxDistance = basicDoor.MainSource.maxDistance * basicDoor.MainSource.maxDistance;
                Vector3 pos = __instance.CastRole.FpcModule.Position;
                Vector3 humanPos = role.FpcModule.Position;

                if (magnitude > maxDistance || __instance.CheckVisibility(player.ReferenceHub) || player.Role == RoleTypeId.Tutorial)
                    return false;

                if (player.TryGetSubclass(out var subclass) && subclass is ICustomSoundSubclass customSound && !customSound.CanSound)
                    return false;

                if (__instance._rippleAssigned)
                {
                    __instance._surfaceRippleTrigger.ProcessRipple(hub);
                    return false;
                }

                __instance.Player.Play(dv.transform.position + DoorRippleTrigger.PosOffset, Color.red);

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
