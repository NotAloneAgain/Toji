using System;
using Toji.BetterCuff.Handlers;
using Toji.ExiledAPI.Configs;

namespace Toji.BetterCuff
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private EventHandlers _handlers;

        public override string Name => "Toji.BetterCuff";

        public override string Prefix => "Toji.BetterCuff";

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
