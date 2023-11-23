using CommandSystem;
using CommandSystem.Commands.RemoteAdmin.Inventory;
using HarmonyLib;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Patches.API.Extensions;
using Utils;

namespace Toji.Patches.Generic.Admins.Items
{
    [HarmonyPatch(typeof(ForceEquipCommand), nameof(ForceEquipCommand.Execute))]
    public static class EquipItemPatch
    {
        private static bool Prefix(ForceEquipCommand __instance, ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
        {
            __result = false;

            if (!Player.TryGet((sender as PlayerCommandSender).ReferenceHub, out var player))
            {
                response = "";

                return true;
            }

            if (!sender.CheckPermission(PlayerPermissions.GivingItems, out response) || player.IsDonator(out _))
            {
                response = "You don't have permissions to execute this command! Required permission: GivingItems";

                return false;
            }

            if (arguments.Count < 2)
            {
                response = "To execute this command provide at least 2 arguments!\nUsage: " + arguments.Array[0] + " " + __instance.DisplayCommandUsage();

                return false;
            }

            List<ReferenceHub> list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out var array, false);

            ItemType itemType = ItemType.None;

            if (array != null && array.Length != 0)
            {
                string text = array[0];

                if (text.Contains('.'))
                {
                    text = text.Split(new char[] { '.' }, StringSplitOptions.None)[0];
                }

                if (int.TryParse(text, out var num) && Enum.IsDefined(typeof(ItemType), num))
                {
                    itemType = (ItemType)num;
                }
            }

            int num2 = 0;
            int num3 = 0;

            if (list != null)
            {
                Func<ReferenceHub, ItemType, bool> func = itemType == ItemType.None ? __instance.TryHolster : __instance.TryEquip;

                foreach (ReferenceHub referenceHub in list)
                {
                    if (func(referenceHub, itemType))
                    {
                        num3++;
                    }
                }
            }

            __result = true;

            response = num3 == 0 ? string.Format("Done! The request affected {0} player{1}", num2, (num2 == 1) ? "!" : "s!") : string.Format("Failed to execute the command! Failures: {0}", num3);

            return true;
        }
    }
}
