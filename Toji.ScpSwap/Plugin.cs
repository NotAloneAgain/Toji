﻿using Exiled.API.Enums;
using Exiled.Events.Handlers;
using System;
using Toji.Global;
using Toji.ScpSwap.Configs;
using Toji.ScpSwap.Handlers;

namespace Toji.ScpSwap
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private SwapHandlers _handlers;

        public override string Name => "Toji.ScpSwap";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override PluginPriority Priority => PluginPriority.High;

        public override void OnEnabled()
        {
            _handlers = new(Config.InfoDuration, Config.InfoText);

            Swap.Slots = Config.Slots;
            Swap.Prevent = Config.PreventMultipleSwaps;
            Swap.AllowedScps = Config.AllowedScps;
            Swap.SwapDuration = Config.SwapDuration;

            Player.ChangingRole += _handlers.OnChangingRole;
            Player.Destroying += _handlers.OnDestroying;

            Server.RestartingRound += _handlers.Reset;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.RestartingRound -= _handlers.Reset;

            Player.Destroying -= _handlers.OnDestroying;
            Player.ChangingRole -= _handlers.OnChangingRole;

            _handlers.Reset();

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
