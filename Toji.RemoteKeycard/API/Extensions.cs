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
            for (var i = 0; i < player.Items.Distinct().Count(); i++)
            {
                InventorySystem.Items.ItemBase item = player.Items.Select(x => x.Base).ElementAt(i);

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
