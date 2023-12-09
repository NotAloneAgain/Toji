using CustomPlayerEffects;
using Exiled.API.Features;
using HarmonyLib;
using Interactables;
using Interactables.Verification;
using InventorySystem.Disarming;
using InventorySystem.Items;
using PlayerRoles.FirstPersonControl;
using System;
using Toji.Classes.API.Extensions;
using Toji.Classes.Subclasses.Abilities.Passive;
using UnityEngine;

namespace Toji.Classes.Patches.Generic
{
    [HarmonyPatch(typeof(StandardDistanceVerification), nameof(StandardDistanceVerification.ServerCanInteract))]
    internal static class VerificationRulePatch
    {
        private static bool Prefix(StandardDistanceVerification __instance, ReferenceHub hub, InteractableCollider collider, out bool __result)
        {
            __result = false;

            try
            {
                if (!__instance._allowHandcuffed && !PlayerInteract.CanDisarmedInteract && hub.inventory.IsDisarmed())
                {
                    return false;
                }

                if (hub.interCoordinator.AnyBlocker(BlockedInteraction.GeneralInteractions))
                {
                    return false;
                }

                if (hub.roleManager.CurrentRole is not IFpcRole fpcRole)
                {
                    return false;
                }

                Transform transform = collider.transform;

                if (Vector3.Distance(fpcRole.FpcModule.Position, transform.position + transform.TransformDirection(collider.VerificationOffset)) > __instance._maxDistance * 1.4f)
                {
                    return false;
                }

                bool hasAbility = Player.TryGet(hub, out var player) && player.TryGetSubclass(out var subclass) && subclass.Abilities.Find(a => a is InvisibleAbility) is InvisibleAbility;

                if (__instance._cancel268 && !hasAbility)
                {
                    hub.playerEffectsController.DisableEffect<Invisible>();
                }

                return false;
            }
            catch (Exception err)
            {
                Log.Error($"[VerificationRulePatch] Error occured: {err}");

                return true;
            }
        }
    }
}
