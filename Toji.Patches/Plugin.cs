using HarmonyLib;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using Toji.Patches.Configs;
using Toji.Patches.Generic.Admins.Forces;
using Toji.Patches.Generic.Admins.Items;
using Toji.Patches.Generic.Sinkhole;

#pragma warning disable IDE0060 // Удалите неиспользуемый параметр

namespace Toji.Patches
{
    public sealed class Plugin
    {
        private const string HarmonyId = "fushiguro";
        private Harmony _harmony;

        [PluginConfig]
        public static Config Config;

        [PluginEntryPoint("Toji.Patches", "1.0.0", "Patches of Toji library", "NotAloneAgain")]
        public void OnEnabled()
        {
            _harmony = new(HarmonyId);

            EventManager.RegisterEvents(this, this);

            _harmony.PatchAll(GetType().Assembly);
        }

        [PluginUnload]
        public void OnDisabled()
        {
            EventManager.UnregisterEvents(this, this);

            OnRestartRound(null);

            _harmony.UnpatchAll(HarmonyId);

            _harmony = null;
        }

        public void OnRestartRound(RoundRestartEvent ev)
        {
            ForceclassPatch.Reset();
            GiveItemPatch.Reset();
            SpawnPatch.Reset();
        }
    }
}
