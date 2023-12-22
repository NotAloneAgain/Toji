using Exiled.Events.Handlers;
using System;
using Toji.Global;
using Toji.NicknameFilters.Configs;
using Toji.NicknameFilters.Handlers;

namespace Toji.NicknameFilters
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        private PlayerHandlers _handlers;

        public override string Name => "Toji.NicknameFilters";

        public override string Prefix => Name.ToPrefix();

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);

        public override void OnEnabled()
        {
            _handlers = new(Config.BadWordsReplacer, Config.BadWordsKick, Config.Ads);

            Player.Verified += _handlers.OnVerified;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Verified -= _handlers.OnVerified;

            _handlers = null;

            base.OnDisabled();
        }

        public override void OnReloaded() { }

        public override void OnRegisteringCommands() { }

        public override void OnUnregisteringCommands() { }
    }
}
