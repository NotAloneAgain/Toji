using Exiled.Events.Handlers;
using System;
using Toji.ExiledAPI.Configs;
using Toji.Global;
using Toji.Pocket.Handlers;

namespace Toji.Pocket
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.Pocket";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Player.StayingOnEnvironmentalHazard += _handlers.OnStayingEnvironmentalHazard;
            Player.EnteringEnvironmentalHazard += _handlers.OnEnteringEnvironmentalHazard;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.EnteringEnvironmentalHazard -= _handlers.OnEnteringEnvironmentalHazard;
            Player.StayingOnEnvironmentalHazard -= _handlers.OnStayingEnvironmentalHazard;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
