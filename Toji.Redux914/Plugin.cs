using System;
using Toji.ExiledAPI.Configs;
using Toji.Global;
using Toji.Redux914.Handlers;

namespace Toji.Redux914
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private Scp914Handlers _handlers;

        public override string Name => "Toji.Redux914";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += _handlers.OnUpgradingInventoryItem;
            Exiled.Events.Handlers.Scp914.UpgradingPickup += _handlers.OnUpgradingPickup;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer += _handlers.OnUpgradingPlayer;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Scp914.UpgradingPlayer -= _handlers.OnUpgradingPlayer;
            Exiled.Events.Handlers.Scp914.UpgradingPickup -= _handlers.OnUpgradingPickup;
            Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= _handlers.OnUpgradingInventoryItem;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
