using Exiled.API.Features;
using HarmonyLib;
using System;
using Toji.Classes.API.Extensions;
using Toji.Classes.Subclasses.Abilities.Passive;

namespace Toji.Classes.Patches.Generic
{
    [HarmonyPatch(typeof(PlayerInteract), nameof(PlayerInteract.OnInteract))]
    internal static class InteractPatch
    {
        private static bool Prefix(PlayerInteract __instance)
        {
            try
            {
                if (!Player.TryGet(__instance._hub, out var player))
                {
                    return true;
                }

                if (!player.TryGetSubclass(out var subclass))
                {
                    return true;
                }

                if (subclass.Abilities.Find(a => a is InvisibleAbility) is not InvisibleAbility)
                {
                    return true;
                }

                return false;
            }
            catch (Exception err)
            {
                Log.Error($"[InteractPatch] Error occured: {err}");

                return true;
            }
        }
    }
}
