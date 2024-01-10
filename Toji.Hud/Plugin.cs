using Exiled.API.Enums;
using Exiled.Events.Handlers;
using System;
using Toji.ExiledAPI.Configs;
using Toji.Global;
using Toji.Hud.Handlers;

namespace Toji.Hud
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _player;
        private Scp096Handlers _scp096;

        public override string Name => "Toji.Hud";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override PluginPriority Priority => PluginPriority.Higher;

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _player = new();
            _scp096 = new();

            Scp096.AddingTarget += _scp096.OnAddingTarget;

            Player.Verified += _player.OnVerified;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Verified -= _player.OnVerified;

            Scp096.AddingTarget -= _scp096.OnAddingTarget;

            _scp096 = null;
            _player = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
