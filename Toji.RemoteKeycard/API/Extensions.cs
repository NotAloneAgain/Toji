using Exiled.API.Features;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Keycards;
using System.Linq;

namespace Toji.RemoteKeycard.API
{
    public static class Extensions
    {
        public static bool CheckPermissions(this Player player, KeycardPermissions perms)
        {
            if (player.IsBypassModeEnabled || player.HasItem(ItemType.KeycardO5) || perms == KeycardPermissions.None || perms == KeycardPermissions.ScpOverride && player.IsScp)
            {
                return true;
            }

            var items = player.Items.Select(x => x.Base);

            for (var i = 0; i < items.Count(); i++)
            {
                InventorySystem.Items.ItemBase item = items.ElementAt(i);

                if (item.Category != ItemCategory.Keycard || item is not KeycardItem card || !card.Permissions.HasFlagFast(perms))
                {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}
