using Exiled.Events.Handlers;
using System;
using Toji.ExiledAPI.Configs;
using Toji.Global;
using Toji.LastPlayers.Handlers;

namespace Toji.LastPlayers
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.LastPlayers";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Player.ChangingRole += _handlers.OnChangingRole;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.ChangingRole -= _handlers.OnChangingRole;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
