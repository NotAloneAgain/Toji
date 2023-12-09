using Exiled.Events.Handlers;
using System;
using Toji.ExiledAPI.Configs;
using Toji.Global;
using Toji.Teslas.Handlers;

namespace Toji.Teslas
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private MapHandlers _map;
        private PlayerHandlers _player;

        public override string Name => "Toji.Teslas";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _map = new();
            _player = new();

            Map.Generated += _map.OnGenerated;

            Player.TriggeringTesla += _player.OnTriggeringTesla;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.TriggeringTesla -= _player.OnTriggeringTesla;

            Map.Generated -= _map.OnGenerated;

            _player = null;
            _map = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
