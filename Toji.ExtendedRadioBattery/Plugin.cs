using Exiled.Events.Handlers;
using System;
using Toji.ExiledAPI.Configs;
using Toji.ExtendedRadioBattery.Handlers;

namespace Toji.ExtendedRadioBattery
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.ExtendedRadioBattery";

        public override string Prefix => "Toji.ExtendedRadioBattery";

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Player.UsingRadioBattery += _handlers.OnUsingRadioBattery;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.UsingRadioBattery -= _handlers.OnUsingRadioBattery;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
