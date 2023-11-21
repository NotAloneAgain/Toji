using HarmonyLib;
using System;
using Toji.Sync.Configs;
using Toji.Sync.Handlers;

namespace Toji.Sync
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private EventHandlers _handlers;

        public override string Name => "Toji.Sync";

        public override string Prefix => "Toji.Sync";

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
