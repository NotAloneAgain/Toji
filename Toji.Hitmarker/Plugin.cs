using Exiled.API.Enums;
using Exiled.Events.Handlers;
using System;
using Toji.Global;
using Toji.Hitmarker.Configs;
using Toji.Hitmarker.Handlers;

namespace Toji.Hitmarker
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.Hitmarker";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override PluginPriority Priority => PluginPriority.Low;

        public override void OnEnabled()
        {
            _handlers = new(Config.DeathTexts);

            Player.Dying += _handlers.OnDying;
            Player.Hurting += _handlers.OnHurting;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Hurting -= _handlers.OnHurting;
            Player.Dying -= _handlers.OnDying;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
