using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using HarmonyLib;
using InventorySystem;
using InventorySystem.Configs;
using PlayerRoles;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using Toji.Patches.API.Extensions;
using Utils;

namespace Toji.Patches.Generic.Admins.Items
{
    [HarmonyPatch(typeof(GiveLoadoutCommand), nameof(GiveLoadoutCommand.Execute))]
    public static class GrantLoadoutPatch
    {
        private static bool Prefix(GiveLoadoutCommand __instance, ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
        {
            __result = false;

            if (!ReferenceHub.TryGetHostHub(out var hostHub))
            {
                response = "You are not connected to a server.";

                return false;
            }

            if (!Player.TryGet((sender as PlayerCommandSender).ReferenceHub, out var player))
            {
                response = "";

                return true;
            }

            if (arguments.Count < 2)
            {
                response = "To execute this command provide at least 2 arguments!\nUsage: " + arguments.Array[0] + " " + __instance.DisplayCommandUsage();

                return false;
            }

            if (!sender.CheckPermission(PlayerPermissions.GivingItems, out response) || player.IsDonator(out _))
            {
                response = "You don't have permissions to execute this command! Required permission: GivingItems";

                return false;
            }

            CharacterClassManager characterClassManager = hostHub.characterClassManager;

            if (characterClassManager == null || !characterClassManager.isLocalPlayer || !characterClassManager.isServer || !characterClassManager.RoundStarted)
            {
                response = "Please start round before using this command.";
                return false;
            }

            List<ReferenceHub> list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out var array, false);

            if (!__instance.TryParseRole(array[0], out var roleBase))
            {
                response = "Invalid role ID / name.";
                return false;
            }

            if (!StartingInventories.DefinedInventories.ContainsKey(roleBase.RoleTypeId))
            {
                response = "Specified role does not have a defined inventory.";
                return false;
            }

            __instance.ProvideRoleFlag(array, out var flags);

            bool flag = flags.HasFlag(RoleSpawnFlags.AssignInventory);

            int num = 0;

            foreach (ReferenceHub referenceHub2 in list)
            {
                if (referenceHub2 != null)
                {
                    InventoryItemProvider.ServerGrantLoadout(referenceHub2, roleBase.RoleTypeId, flag);
                    AddLog(2, string.Format("{0} gave {1} the loadout of {2}.", sender.LogName, referenceHub2.LoggedNameFromRefHub(), roleBase.RoleTypeId), 1, false);
                    num++;
                }
            }

            __result = true;

            response = string.Format("Done! Given {0}'s loadout to {1} player{2}!", roleBase.RoleName, num, (num == 1) ? "" : "s");

            return true;
        }

        private static void AddLog(int module, string msg, int type, bool init = false)
        {
            var text = TimeBehaviour.Rfc3339Time();

            var lockObject = ServerLogs.LockObject;

            lock (lockObject)
            {
                ServerLogs.Queue.Enqueue(new(msg, ServerLogs.Txt[type], ServerLogs.Modulestxt[module], text));
            }

            if (init)
                return;

            ServerLogs._state = ServerLogs.LoggingState.Write;
        }
    }
}
