using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using System.Linq;

namespace Toji.ExiledAPI.Extensions
{
    public static class PlayerExtensions
    {
        public static bool HasEffect<TEffect>(this Player player) where TEffect : StatusEffectBase => player != null && player.IsEffectActive<TEffect>();

        public static void DropAllWithoutKeycard(this Player player)
        {
            if (player.IsInventoryEmpty)
            {
                return;
            }

            foreach (Item item in player.Items.ToHashSet())
            {
                if (item.IsKeycard)
                {
                    continue;
                }

                player.DropItem(item);
            }
        }
    }
}
