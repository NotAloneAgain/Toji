using Exiled.API.Enums;
using Exiled.Events.Handlers;
using System;
using Toji.ExiledAPI.Configs;
using Toji.Global;
using Toji.MoreRealistic.Handlers;

namespace Toji.MoreRealistic
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.MoreRealistic";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override PluginPriority Priority => PluginPriority.Last;

        public override void OnEnabled()
        {
            _handlers = new();

            Player.Hurting += _handlers.OnHurting;
            Player.Dying += _handlers.OnDying;
            Player.Shot += _handlers.OnShot;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Shot -= _handlers.OnShot;
            Player.Dying -= _handlers.OnDying;
            Player.Hurting -= _handlers.OnHurting;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
