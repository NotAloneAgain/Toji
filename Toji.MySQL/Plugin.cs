using System;
using Toji.Global;
using Toji.MySQL.Configs;
using Toji.MySQL.Handlers;

namespace Toji.MySQL
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private EventHandlers _handlers;

        public override string Name => "Toji.MySQL";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
