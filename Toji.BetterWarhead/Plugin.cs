﻿using Exiled.Events.Handlers;
using System;
using Toji.BetterWarhead.Handlers;
using Toji.ExiledAPI.Configs;
using Toji.Global;

namespace Toji.BetterWarhead
{
    public sealed class Plugin : Exiled.API.Features.Plugin<DefaultConfig>
    {
        private WarheadHandlers _handlers;

        public override string Name => "Toji.BetterWarhead";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new();

            Warhead.Detonating += _handlers.OnDetonating;
            Warhead.Detonated += _handlers.OnDetonated;
            Warhead.Starting += _handlers.OnStarting;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _handlers = null;

            Warhead.Starting -= _handlers.OnStarting;
            Warhead.Detonated -= _handlers.OnDetonated;
            Warhead.Detonating -= _handlers.OnDetonating;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
