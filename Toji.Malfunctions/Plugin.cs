using Exiled.Events.Handlers;
using System;
using Toji.Global;
using Toji.Malfunctions.Configs;
using Toji.Malfunctions.Handlers;

namespace Toji.Malfunctions
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private ServerHandlers _handlers;

        // TO-DO:
        // Помимо выключения света и перезапуска системы дверей добавить поломки лифтов/дверей, переезд лужи деда в офисы и изоляцию комнат.

        public override string Name => "Toji.Malfunctions";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Server.RestartingRound += _handlers.OnRestartingRound;
            Server.RoundStarted += _handlers.OnRoundStarted;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
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
