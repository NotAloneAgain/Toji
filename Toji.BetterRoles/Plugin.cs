using Exiled.Events.Handlers;
using System;
using Toji.BetterRoles.Handlers;
using Toji.ExiledAPI.Configs;
using Toji.Global;

namespace Toji.BetterRoles
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private PlayerHandlers _player;
        private Scp3114Handlers _skeleton;

        public override string Name => "Toji.BetterRoles";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _player = new();
            _skeleton = new();

            Scp3114.Disguised += _skeleton.OnDisguised;

            Player.EnteringPocketDimension += _player.OnEnteringPocketDimension;
            Player.ChangingRole += _player.OnChangingRole;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.ChangingRole -= _player.OnChangingRole;
            Player.EnteringPocketDimension -= _player.OnEnteringPocketDimension;

            Scp3114.Disguised -= _skeleton.OnDisguised;

            _player = null;
            _skeleton = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
