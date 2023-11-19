using InventorySystem;
using InventorySystem.Items;

namespace Toji.PluginAPI.API.Extensions
{
    public static class ItemExtensions
    {
        public static bool IsKeycard(this ItemType type) => type.GetCategory() == ItemCategory.Keycard;

        public static bool IsAmmo(this ItemType type) => type.GetCategory() == ItemCategory.Ammo;

        public static bool IsArmor(this ItemType type) => type.GetCategory() == ItemCategory.Armor;

        public static bool IsScp(this ItemType type) => type.GetCategory() == ItemCategory.SCPItem;

        public static bool IsMedical(this ItemType type) => type.GetCategory() == ItemCategory.Medical;

        public static bool IsGrenade(this ItemType type) => type.GetCategory() == ItemCategory.Grenade;

        public static bool IsFirearm(this ItemType type) => type.GetCategory() == ItemCategory.Firearm;

        public static bool IsHid(this ItemType type) => type.GetCategory() == ItemCategory.MicroHID;

        public static bool IsRadio(this ItemType type) => type.GetCategory() == ItemCategory.Radio;

        public static bool IsWeapon(this ItemType type) => type.GetCategory() is ItemCategory.Firearm or ItemCategory.MicroHID;

        public static bool IsDangerous(this ItemType type) => type.IsFirearm() || type.IsHid() || type.IsGrenade() || type is ItemType.SCP018 or ItemType.SCP244a or ItemType.SCP244b or ItemType.SCP330;

        public static bool IsSafe(this ItemType type) => !type.IsDangerous();

        public static ItemBase GetItemBase(this ItemType type)
        {
            if (!InventoryItemLoader.AvailableItems.TryGetValue(type, out ItemBase itemBase))
                return null;

            return itemBase;
        }

        public static T GetItemBase<T>(this ItemType type)
            where T : ItemBase
        {
            if (!InventoryItemLoader.AvailableItems.TryGetValue(type, out ItemBase itemBase))
                return null;

            return itemBase as T;
        }

        public static ItemCategory GetCategory(this ItemType type) => type.GetItemBase().Category;
    }
}
