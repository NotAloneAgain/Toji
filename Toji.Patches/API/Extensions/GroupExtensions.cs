using System.Collections.Generic;

namespace Toji.Patches.API.Extensions
{
    public static class GroupExtensions
    {
        public static bool IsDonator(this string group) => !string.IsNullOrEmpty(group) && Plugin.Config.DonatorRoles.Contains(group);

        public static int GetForcesLimit(this string group) => group.IsDonator() ? Plugin.Config.ForceLimits.TryGetValue(group, out var value) ? value : 2 : 0;

        public static int GetItemsLimit(this string group) => group.IsDonator() ? Plugin.Config.ItemLimits.TryGetValue(group, out var value) ? value : 2 : 0;

        public static string GetNameByGroup(this UserGroup group)
        {
            PermissionsHandler handler = ServerStatic.GetPermissionsHandler();

            Dictionary<string, UserGroup> groups = handler.GetAllGroups();

            KeyValuePair<string, UserGroup>? pair = null!;

            foreach (KeyValuePair<string, UserGroup> gru in groups)
            {
                (var key, UserGroup g) = (gru.Key, gru.Value);

                if (g.Permissions == group.Permissions && g.RequiredKickPower == group.RequiredKickPower && g.KickPower == group.KickPower && g.BadgeText == group.BadgeText && g.BadgeColor == group.BadgeColor)
                {
                    pair = gru;

                    break;
                }
            }

            return pair is null ? string.Empty : pair.Value.Key;
        }
    }
}
