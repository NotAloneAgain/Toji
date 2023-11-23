using Exiled.API.Features;
using Toji.Classes.API.Features;

namespace Toji.Classes.API.Extensions
{
    public static class PlayerExtensions
    {
        public static BaseSubclass GetSubclass(this Player player) => BaseSubclass.Get(player);

        public static TSubclass GetSubclass<TSubclass>(this Player player) where TSubclass : BaseSubclass => BaseSubclass.Get<TSubclass>(player);

        public static bool TryGetSubclass(this Player player, out BaseSubclass subclass) => BaseSubclass.TryGet(player, out subclass);

        public static bool TryGetSubclass<TSubclass>(this Player player, out TSubclass subclass) where TSubclass : BaseSubclass => BaseSubclass.TryGet(player, out subclass);
    }
}
