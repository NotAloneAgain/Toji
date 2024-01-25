using HarmonyLib;
using PluginAPI.Core;
using RemoteAdmin;
using RemoteAdmin.Communication;
using System;
using Toji.Patches.API.Extensions;

#pragma warning disable IDE0060

namespace Toji.Patches.Generic.Admins
{
    [HarmonyPatch(typeof(RaClientDataRequest), nameof(RaClientDataRequest.ReceiveData), new Type[2] { typeof(CommandSender), typeof(string) })]
    internal static class RespawnTimePatch
    {
        private static bool Prefix(RaClientDataRequest __instance, CommandSender sender, string data)
        {
            if (__instance is not RaTeamStatus)
                return true;

            if (!Player.TryGet((sender as PlayerCommandSender).ReferenceHub, out var player))
                return true;

            var group = player.ReferenceHub.serverRoles.Group;

            if (group.KickPower <= 0 || group.GetNameByGroup() == "admin-junior")
            {
                __instance._stringBuilder.Clear();
                __instance._stringBuilder.Append("$").Append(__instance.DataId).Append(" ");
                __instance.AppendData(0);
                __instance.AppendData(0);
                __instance.AppendData(0);
                __instance.AppendData(0);
                sender.RaReply(string.Format("${0} {1}", __instance.DataId, __instance._stringBuilder), true, false, string.Empty);

                return false;
            }

            __instance._stringBuilder.Clear();
            __instance._stringBuilder.Append("$").Append(__instance.DataId).Append(" ");
            __instance.GatherData();
            sender.RaReply(string.Format("${0} {1}", __instance.DataId, __instance._stringBuilder), true, false, string.Empty);

            return false;
        }
    }
}
