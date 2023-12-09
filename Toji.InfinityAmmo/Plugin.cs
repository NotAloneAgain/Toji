using Exiled.Events.Handlers;
using System;
using System.Linq;
using Toji.ExiledAPI.Configs;
using Toji.Global;
using Toji.InfinityAmmo.Handlers;

namespace Toji.InfinityAmmo
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.InfinityAmmo";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Player.ReloadingWeapon += _handlers.OnReloadingWeapon;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.ReloadingWeapon -= _handlers.OnReloadingWeapon;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
