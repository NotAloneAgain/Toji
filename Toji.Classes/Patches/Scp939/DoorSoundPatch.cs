using CustomPlayerEffects;
using Exiled.API.Features;
using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp939.Ripples;
using System;
using Toji.Classes.API.Extensions;
using Toji.Classes.Subclasses.Characteristics;
using Toji.ExiledAPI.Extensions;
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

                if (player.Role.Team is Team.Dead or Team.OtherAlive or Team.SCPs)
                    return false;

                var role = player.Role.Base as HumanRole;

                if (player.HasEffect<Invisible>() || role == null)
                    return false;

                var magnitude = (dv.transform.position + DoorRippleTrigger.PosOffset - __instance.CastRole.FpcModule.Position).sqrMagnitude;
                var maxDistance = basicDoor.MainSource.maxDistance * basicDoor.MainSource.maxDistance;
                Vector3 pos = __instance.CastRole.FpcModule.Position;
                Vector3 humanPos = role.FpcModule.Position;

                if (magnitude > maxDistance || __instance.CheckVisibility(player.ReferenceHub) || player.Role == RoleTypeId.Tutorial)
                    return false;

                if (player.TryGetSubclass(out var subclass) && subclass.Characteristics.Find(x => x is SoundCharacteristic) is SoundCharacteristic sound && sound.Value)
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
                Log.Error($"[DoorSoundPatch] Error occured: {err}");

                return true;
            }
        }
    }
}
