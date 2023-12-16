using Exiled.Events.Handlers;
using System;
using Toji.BetterWarhead.Handlers;
using Toji.ExiledAPI.Configs;
using Toji.Global;

namespace Toji.BetterWarhead
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private WarheadHandlers _warhead;
        private ServerHandlers _server;

        public override string Name => "Toji.BetterWarhead";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _warhead = new();
            _server = new();

            Server.RestartingRound += _server.OnRestartingRound;

            Warhead.Detonating += _warhead.OnDetonating;
            Warhead.Detonated += _warhead.OnDetonated;
            Warhead.Starting += _warhead.OnStarting;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Warhead.Starting -= _warhead.OnStarting;
            Warhead.Detonated -= _warhead.OnDetonated;
            Warhead.Detonating -= _warhead.OnDetonating;

            Server.RestartingRound -= _server.OnRestartingRound;

            _server = null;
            _warhead = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
