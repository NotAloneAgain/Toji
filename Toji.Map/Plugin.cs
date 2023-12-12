using Exiled.Events.Handlers;
using System;
using Toji.BetterMap.Handlers;
using Toji.ExiledAPI.Configs;
using Toji.Global;

namespace Toji.BetterMap
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private MapHandlers _handlers;

        public override string Name => "Toji.Map";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Map.GeneratorActivating += _handlers.OnGeneratorActivated;
            Map.Generated += _handlers.OnGenerated;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Map.Generated -= _handlers.OnGenerated;
            Map.GeneratorActivating -= _handlers.OnGeneratorActivated;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
