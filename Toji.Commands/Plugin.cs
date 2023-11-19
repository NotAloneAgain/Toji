using System;
using Toji.Commands.Configs;

namespace Toji.Commands
{
    public sealed class Plugin : Exiled.API.Features.Plugin<Config>
    {
        public override string Name => "Toji.Commands";

        public override string Prefix => "Toji.Commands";

        public override string Author => "NotAloneAgain";

        public override Version Version { get; } = new(1, 0, 0);
    }
}
