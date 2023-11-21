using Exiled.Events.Handlers;
using System;
using Toji.ExiledAPI.Configs;
using Toji.RemoteKeycard.Handlers;

namespace Toji.RemoteKeycard
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.RemoteKeycard";

        public override string Prefix => "Toji.RemoteKeycard";

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Player.UnlockingGenerator += _handlers.OnUnlockingGenerator;
            Player.InteractingLocker += _handlers.OnInteractingLocker;
            Player.InteractingDoor += _handlers.OnInteractingDoor;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.InteractingDoor -= _handlers.OnInteractingDoor;
            Player.InteractingLocker -= _handlers.OnInteractingLocker;
            Player.UnlockingGenerator -= _handlers.OnUnlockingGenerator;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
