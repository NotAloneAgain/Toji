using Exiled.Events.Handlers;
using System;
using Toji.AutoFF.Handlers;
using Toji.ExiledAPI.Configs;
using Toji.Global;

namespace Toji.AutoFF
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private RoundHandlers _handlers;

        public override string Name => "Toji.AutoFF";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Server.RestartingRound += _handlers.OnRestartingRound;
            Server.RoundStarted += _handlers.OnRoundStarted;
            Server.EndingRound += _handlers.OnEndingRound;
            Server.RoundEnded += _handlers.OnRoundEnded;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.RoundEnded -= _handlers.OnRoundEnded;
            Server.EndingRound -= _handlers.OnEndingRound;
            Server.RoundStarted -= _handlers.OnRoundStarted;
            Server.RestartingRound -= _handlers.OnRestartingRound;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
