using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Toji.ExiledAPI.Configs
{
    public sealed class DefaultConfig : IConfig
    {
        [Description("Enabled or not.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debug enabled or not.")]
        public bool Debug { get; set; } = false;
    }
}
