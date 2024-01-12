using Exiled.API.Enums;
using Exiled.Events.Handlers;
using System;
using Toji.ExiledAPI.Configs;
using Toji.Global;
using Toji.ReduxRespawning.Handlers;

namespace Toji.ReduxRespawning
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private ServerHandlers _server;
        private PlayerHandlers _player;

        public override string Name => "Toji.ReduxRespawning";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override PluginPriority Priority => PluginPriority.Higher;

        public override void OnEnabled()
        {
            _server = new();
            _player = new();

            Player.ChangingRole += _player.OnChangingRole;

            Server.SelectingRespawnTeam += _server.OnSelectingTeam;
            Server.RoundStarted += _server.OnRoundStarted;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.RoundStarted -= _server.OnRoundStarted;
            Server.SelectingRespawnTeam -= _server.OnSelectingTeam;

            Player.ChangingRole -= _player.OnChangingRole;

            _player = null;
            _server = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
