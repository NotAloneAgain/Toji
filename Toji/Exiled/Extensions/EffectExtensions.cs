using CustomPlayerEffects;
using Exiled.API.Features;
using Interactables.Interobjects.DoorUtils;

namespace Toji.ExiledAPI.Extensions
{
    public static class EffectExtensions
    {
        public static bool HasEffect<TEffect>(this Player player) where TEffect : StatusEffectBase
        {
            return player != null && player.TryGetEffect(out TEffect effect) && effect.IsEnabled;
        }

        public static bool HasFlagFast(this KeycardPermissions perm, KeycardPermissions perm2)
        {
            return (perm & perm2) == perm2;
        }
    }
}
