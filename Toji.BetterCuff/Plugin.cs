using Exiled.Events.Handlers;
using System;
using Toji.BetterCuff.Handlers;
using Toji.ExiledAPI.Configs;

namespace Toji.BetterCuff
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.BetterCuff";

        public override string Prefix => "Toji.BetterCuff";

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Player.Hurting += _handlers.OnHurting;
            Player.Dying += _handlers.OnDying;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Dying -= _handlers.OnDying;
            Player.Hurting -= _handlers.OnHurting;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
