using HarmonyLib;
using InventorySystem.Items;
using PlayerRoles;

namespace Toji.Patches.Generic.Players
{
    [HarmonyPatch(typeof(HumanRole), nameof(HumanRole.AllowDisarming))]
    internal class DisarmingRulesPatch
    {
        private static bool Prefix(HumanRole __instance, ReferenceHub detainer, ref bool __result)
        {
            __result = true;

            var faction = __instance.Team.GetFaction();
            var targetFaction = detainer.GetFaction();

            if (faction == targetFaction)
            {
                __result = __instance.RoleTypeId == RoleTypeId.ClassD && detainer.roleManager.CurrentRole.RoleTypeId == RoleTypeId.ClassD;
            }

            if (!__instance.TryGetOwner(out var hub))
            {
                __result = true;
            }

            if (hub.interCoordinator.AnyBlocker(BlockedInteraction.BeDisarmed))
            {
                __result = false;
            }

            return false;
        }
    }
}
