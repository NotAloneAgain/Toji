using HarmonyLib;
using System;
using Toji.Redux914.Configs;
using Toji.Redux914.Handlers;

namespace Toji.Redux914
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private EventHandlers _handlers;

        public override string Name => "Toji.Redux914";

        public override string Prefix => "Toji.Redux914";

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
