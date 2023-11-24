using CentralAuth;
using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerRoles;
using PluginAPI.Core;
using RemoteAdmin;
using RemoteAdmin.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toji.Patches.API.Extensions;
using VoiceChat;

namespace Toji.Patches.Generic.Admins
{
    [HarmonyPatch(typeof(RaPlayerList), nameof(RaPlayerList.ReceiveData), new Type[2] { typeof(CommandSender), typeof(string) })]
    internal static class PlayerListPatch
    {
        private static bool Prefix(RaPlayerList __instance, CommandSender sender, string data)
        {
            var array = data.Split(new char[] { ' ' }, StringSplitOptions.None);

            if (array.Length != 3)
                return false;

            if (!int.TryParse(array[0], out var num) || !byte.TryParse(array[1], out var sortNum))
                return false;

            if (sortNum is < 0 or > 3)
                return false;

            var flag = num == 1;
            var descending = array[2].Equals("1");

            var canHidden = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenBadges);
            var canGlobal = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenGlobalBadges);

            var playerSender = sender as PlayerCommandSender;
            var senderHub = playerSender?.ReferenceHub;

            if (!Player.TryGet(senderHub, out var player))
                return true;

            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent("\n");

            foreach (ReferenceHub referenceHub in SortBy(descending, sortNum))
            {
                var fullyHidden = referenceHub.authManager.UserId == "76561199011540209@steam" && player.UserId != "76561199011540209@steam";

                if (referenceHub.Mode is ClientInstanceMode.DedicatedServer or ClientInstanceMode.Unverified)
                    continue;

                if (player.IsDonator(out _) && referenceHub.authManager.UserId != player.UserId)
                    continue;

                var prefix = fullyHidden ? string.Empty : RaPlayerList.GetPrefix(referenceHub, canHidden, canGlobal);

                stringBuilder.Append(prefix);

                if (!fullyHidden && referenceHub.serverRoles.IsInOverwatch)
                    stringBuilder.Append("<link=RA_OverwatchEnabled><color=white>[</color><color=#03f8fc>\uf06e</color><color=white>]</color></link> ");

                if (!fullyHidden && VoiceChatMutes.IsMuted(referenceHub, false))
                    stringBuilder.Append("<link=RA_Muted><color=white>[</color>\ud83d\udd07<color=white>]</color></link> ");

                if (fullyHidden)
                {
                    stringBuilder.Append("<color=white>(").Append(referenceHub.PlayerId).Append(") ");

                    stringBuilder.Append(referenceHub.nicknameSync.CombinedName.Replace("\n", string.Empty).Replace("RA_", string.Empty)).Append("</color>");

                    stringBuilder.AppendLine();

                    continue;
                }

                stringBuilder.Append("<color={RA_ClassColor}>(").Append(referenceHub.PlayerId).Append(") ");

                stringBuilder.Append(referenceHub.nicknameSync.CombinedName.Replace("\n", string.Empty).Replace("RA_", string.Empty)).Append("</color>");
                stringBuilder.AppendLine();
            }

            sender.RaReply(string.Format("${0} {1}", __instance.DataId, StringBuilderPool.Shared.ToStringReturn(stringBuilder)), true, !flag, string.Empty);

            return false;
        }

        private static IEnumerable<ReferenceHub> SortBy(bool descending, int type)
        {
            switch (descending)
            {
                case true:
                    {
                        return type switch
                        {
                            1 => ReferenceHub.AllHubs.OrderByDescending(hub => hub.nicknameSync.DisplayName ?? hub.nicknameSync.MyNick),
                            3 => ReferenceHub.AllHubs.OrderByDescending(hub => hub.GetTeam()),
                            2 => ReferenceHub.AllHubs.OrderByDescending(hub => hub.GetTeam()).ThenByDescending(hub => hub.GetRoleId()),
                            _ => ReferenceHub.AllHubs.OrderByDescending(hub => hub.PlayerId),
                        };
                    }
                case false:
                    {
                        return type switch
                        {
                            1 => ReferenceHub.AllHubs.OrderBy(hub => hub.nicknameSync.DisplayName ?? hub.nicknameSync.MyNick),
                            3 => ReferenceHub.AllHubs.OrderBy(hub => hub.GetTeam()),
                            2 => ReferenceHub.AllHubs.OrderBy(hub => hub.GetTeam()).ThenBy(hub => hub.GetRoleId()),
                            _ => ReferenceHub.AllHubs.OrderBy(hub => hub.PlayerId),
                        };
                    }
            }
        }
    }
}
