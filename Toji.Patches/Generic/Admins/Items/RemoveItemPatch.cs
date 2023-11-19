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
    [HarmonyPatch(typeof(RemoveItemCommand), nameof(RemoveItemCommand.Execute))]
    public static class RemoveItemPatch
    {
        private static bool Prefix(RemoveItemCommand __instance, ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
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

            if (array == null || array.Length == 0)
            {
                response = "You must specify item(s) to give.";
                return false;
            }

            ItemType[] array2 = __instance.ParseItems(array[0]).ToArray();

            if (array2.Length == 0)
            {
                response = "You didn't input any items.";
                return false;
            }

            int num = 0;
            int num2 = 0;
            string text = string.Empty;

            if (list != null)
            {
                foreach (ReferenceHub referenceHub in list)
                {
                    try
                    {
                        foreach (ItemType itemType in array2)
                        {
                            __instance.RemoveItem(referenceHub, sender, itemType);
                        }
                    }
                    catch (Exception ex)
                    {
                        num++;
                        text = ex.Message;
                        continue;
                    }
                    num2++;
                }
            }

            __result = true;

            response = num == 0 ? string.Format("Done! The request affected {0} player{1}", num2, (num2 == 1) ? "!" : "s!") : string.Format("Failed to execute the command! Failures: {0}\nLast error log:\n{1}", num, text);

            return true;
        }
    }
}
