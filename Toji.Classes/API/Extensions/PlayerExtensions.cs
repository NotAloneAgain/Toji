using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;

namespace Toji.Classes.API.Extensions
{
    public static class PlayerExtensions
    {
        public static HashSet<string> Blacklist { get; internal set; }

        public static BaseSubclass GetSubclass(this Player player) => BaseSubclass.Get(player);

        public static TSubclass GetSubclass<TSubclass>(this Player player) where TSubclass : BaseSubclass => BaseSubclass.Get<TSubclass>(player);

        public static bool TryGetSubclass(this Player player, out BaseSubclass subclass) => BaseSubclass.TryGet(player, out subclass);

        public static bool TryGetSubclass<TSubclass>(this Player player, out TSubclass subclass) where TSubclass : BaseSubclass => BaseSubclass.TryGet(player, out subclass);

        public static bool GrantSubclass(this Player player, ref RoleTypeId role)
        {
            if (!BaseSubclass.TryGet(role, out var subclasses))
            {
                return false;
            }

            foreach (var sub in subclasses)
            {
                if (sub == null || !sub.Can(player) || Blacklist.Contains(sub.Name))
                {
                    continue;
                }

                role = sub.SpawnRules.Model;

                sub.DelayedAssign(player);

                return true;
            }

            return false;
        }

        public static bool GrantSubclass(this Player player, RoleTypeId role)
        {
            if (!BaseSubclass.TryGet(role, out var subclasses))
            {
                return false;
            }

            foreach (var sub in subclasses)
            {
                if (sub == null || !sub.Can(player) || Blacklist.Contains(sub.Name))
                {
                    continue;
                }

                player.Role.Set(role, SpawnReason.None, RoleSpawnFlags.None);

                sub.DelayedAssign(player);

                return true;
            }

            return false;
        }
    }
}
