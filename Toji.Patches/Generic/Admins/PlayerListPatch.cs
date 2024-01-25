using CentralAuth;
using Discord;
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
using System.Threading;
using Toji.Patches.API.Extensions;
using VoiceChat;

namespace Toji.Patches.Generic.Admins
{
    [HarmonyPatch(typeof(RaPlayerList), nameof(RaPlayerList.ReceiveData), [typeof(CommandSender), typeof(string)])]
    internal static class PlayerListPatch
    {
        private static bool IsFullyHidden(bool isDonator, string userId, string targetUserId) => userId != targetUserId && (isDonator || targetUserId == "76561199011540209@steam");

        private static bool Prefix(RaPlayerList __instance, CommandSender sender, string data)
        {
            var array = data.Split(new char[] { ' ' }, StringSplitOptions.None);

            if (array.Length != 3)
                return false;

            if (!int.TryParse(array[0], out var num) || !byte.TryParse(array[1], out var sortNum))
                return false;

            if (sortNum is < 0 or > 3)
                return false;

            var shouldConsoleLog = num == 1;
            var isDescending = array[2].Equals("1");

            var playerSender = sender as PlayerCommandSender;
            var senderHub = playerSender?.ReferenceHub;

            if (!Player.TryGet(senderHub, out var player))
            {
                return true;
            }

            bool result;
            try
            {
                Handle(__instance, player, sender, shouldConsoleLog, isDescending, sortNum);

                result = false;
            }
            catch (Exception err)
            {
                Log.Error($"[PlayerListPatch] Error for {player.Nickname} ({player.UserId}): {err}");

                result = true;
            }

            return result;
        }

        private static void Handle(RaPlayerList instance, Player sender, CommandSender commandSender, bool shouldConsoleLog, bool isDescending, int sortType)
        {
            var donator = sender.IsDonator(out _);

            StringBuilder result = StringBuilderPool.Shared.Rent("\n");

            if (donator)
            {
                HandleDonator(sender, ref result);
            }
            else
            {
                HandleAdmin(sender, commandSender, isDescending, sortType, ref result);
            }

            commandSender.RaReply(string.Format("${0} {1}", instance.DataId, StringBuilderPool.Shared.ToStringReturn(result)), true, shouldConsoleLog, string.Empty);
        }

        private static void HandleDonator(Player sender, ref StringBuilder builder)
        {
            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                if (hub.Mode != ClientInstanceMode.ReadyClient || hub.authManager.UserId != sender.UserId)
                    continue;

                var nickname = hub.nicknameSync.CombinedName.Replace("\n", string.Empty).Replace("RA_", string.Empty);

                builder.Append("<color=#63b9c7> ");

                builder.Append(nickname);

                builder.Append("</color>");

                builder.AppendLine();
            }
        }

        private static void HandleAdmin(Player sender, CommandSender commandSender, bool isDescending, int sortType, ref StringBuilder builder)
        {
            builder.Append("<size=20>(Отменить выделение)</size>\n");

            Func<string, bool> isFullyHidden = (string targetId) => IsFullyHidden(false, sender.UserId, targetId);

            (bool canViewHidden, bool canViewGlobal) = (CommandProcessor.CheckPermissions(commandSender, PlayerPermissions.ViewHiddenBadges), CommandProcessor.CheckPermissions(commandSender, PlayerPermissions.ViewHiddenGlobalBadges));

            foreach (ReferenceHub hub in SortHubs(isDescending, sortType))
            {
                if (hub.Mode is ClientInstanceMode.DedicatedServer or ClientInstanceMode.Unverified)
                    continue;

                var nickname = hub.nicknameSync.CombinedName.Replace("\n", string.Empty).Replace("RA_", string.Empty);

                string color = "white";

                if (!isFullyHidden(hub.authManager.UserId))
                {
                    color = "{RA_ClassColor}";

                    builder.Append(RaPlayerList.GetPrefix(hub, canViewHidden, canViewGlobal));

                    if (hub.serverRoles.IsInOverwatch)
                    {
                        builder.Append("<link=RA_OverwatchEnabled><color=white>[</color><color=#03f8fc>\uf06e</color><color=white>]</color></link> ");
                    }

                    if (VoiceChatMutes.IsMuted(hub, false))
                    {
                        builder.Append("<link=RA_Muted><color=white>[</color>\ud83d\udd07<color=white>]</color></link> ");
                    }
                }

                builder.Append("<color=").Append(color).Append(">(").Append(hub.PlayerId).Append(") ");

                builder.Append(nickname);

                builder.Append("</color>");

                builder.AppendLine();
            }
        }

        private static IEnumerable<ReferenceHub> SortHubs(bool isDescending, int sortType)
        {
            switch (isDescending)
            {
                case true:
                    {
                        return sortType switch
                        {
                            1 => ReferenceHub.AllHubs.OrderByDescending(hub => hub.nicknameSync.DisplayName ?? hub.nicknameSync.MyNick),
                            3 => ReferenceHub.AllHubs.OrderByDescending(hub => hub.GetTeam()),
                            2 => ReferenceHub.AllHubs.OrderByDescending(hub => hub.GetTeam()).ThenByDescending(hub => hub.GetRoleId()),
                            _ => ReferenceHub.AllHubs.OrderByDescending(hub => hub.PlayerId),
                        };
                    }
                case false:
                    {
                        return sortType switch
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
