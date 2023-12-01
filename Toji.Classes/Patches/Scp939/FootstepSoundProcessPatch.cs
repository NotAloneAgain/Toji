using AudioPooling;
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
using Utils.Networking;

namespace Toji.Patches.Generic.Scp939
{
    [HarmonyPatch(typeof(FootstepRippleTrigger), nameof(FootstepRippleTrigger.ClientProcessRpc))]
    internal static class FootstepSoundProcessPatch
    {
        private static bool Prefix(FootstepRippleTrigger __instance, NetworkReader reader)
        {
            try
            {
                if (__instance == null || reader == null || !NetworkServer.active)
                {
                    return false;
                }

                if (!reader.TryReadReferenceHub(out __instance._syncPlayer))
                {
                    return false;
                }

                if (__instance._syncPlayer == null || __instance._syncPlayer.gameObject == null || !Player.TryGet(__instance._syncPlayer.gameObject, out Player player))
                {
                    return false;
                }

                if (player.Role.Team is Team.Dead or Team.OtherAlive or Team.SCPs)
                {
                    return false;
                }

                var role = player.Role.Base as HumanRole;

                if (role == null || player.Role == RoleTypeId.Tutorial)
                {
                    return false;
                }

                var model = role.FpcModule.CharacterModelInstance as AnimatedCharacterModel;

                if (model == null)
                {
                    return false;
                }

                if (player.TryGetSubclass(out var subclass) && subclass.Characteristics.Find(x => x is SoundCharacteristic) is SoundCharacteristic sound && sound.Value)
                {
                    return false;
                }

                __instance._syncPos = reader.ReadRelativePosition();
                __instance._syncDistance = reader.ReadByte();
                __instance.Player.Play(__instance._syncPos.Position, role.RoleColor);
                _ = AudioSourcePoolManager.PlaySound(model.RandomFootstep, __instance._syncPos.Position, __instance._syncDistance, 1, FalloffType.Exponential, AudioMixerChannelType.DefaultSfx, 1, false);
                __instance.OnPlayedRipple(__instance._syncPlayer);

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
