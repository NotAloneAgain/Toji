using Exiled.Events.Handlers;
using System;
using Toji.BetterEscape.Handlers;
using Toji.ExiledAPI.Configs;
using Toji.Global;

namespace Toji.BetterEscape
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.BetterEscape";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Player.Spawned += _handlers.OnSpawned;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Spawned -= _handlers.OnSpawned;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
