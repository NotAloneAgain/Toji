using System.Collections.Generic;
using Toji.ExiledAPI.Configs;

namespace Toji.Classes.Configs
{
    public sealed class Config : DefaultConfig
    {
        public List<string> BlacklistedSubclasses { get; set; } = new List<string>()
        {
            "abc",
            "cba"
        };
    }
}
