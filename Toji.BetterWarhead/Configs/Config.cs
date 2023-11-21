﻿using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Toji.BetterWarhead.Configs
{
    public sealed class Config : IConfig
    {
        [Description("Enabled or not.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug enabled or not.")]
        public bool Debug { get; set; } = false;

        public bool OffLights { get; set; } = true;
    }
}
