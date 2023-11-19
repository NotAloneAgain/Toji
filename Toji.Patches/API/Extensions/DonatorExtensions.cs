using PluginAPI.Core;

namespace Toji.Patches.API.Extensions
{
    public static class DonatorExtensions
    {
        public static bool IsDonator(this Player player, out string donatorGroup)
        {
            var groupName = player.ReferenceHub.serverRoles.Group.GetNameByGroup();
            var otherName = ServerStatic.GetPermissionsHandler().GetUserGroup(player.UserId).GetNameByGroup();

            if (groupName.IsDonator())
            {
                donatorGroup = groupName;

                return true;
            }

            if (otherName.IsDonator())
            {
                donatorGroup = otherName;

                return true;
            }

            donatorGroup = string.Empty;

            return false;
        }
    }
}
