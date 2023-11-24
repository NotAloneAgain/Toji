using CustomPlayerEffects;
using PluginAPI.Core;

namespace Toji.NwPluginAPI.API.Extensions
{
    public static class PlayerExtensions
    {
        public static bool HasEffect<TEffect>(this Player player) where TEffect : StatusEffectBase => player != null && player.EffectsManager.TryGetEffect(out TEffect effect) && effect.IsEnabled;
    }
}
