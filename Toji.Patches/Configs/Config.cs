using System.Collections.Generic;
using System.ComponentModel;

namespace Toji.Patches.Configs
{
    public sealed class Config
    {
        [Description("Включено ли?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Роли донатеров.")]
        public List<string> DonatorRoles { get; set; } = new List<string>()
        {
            "donate-fifth",
            "donate-fourth",
            "donate-third",
            "donate-second",
            "donate-first",
        };

        [Description("Лимиты по форсу для донатеров.")]
        public Dictionary<string, int> ForceLimits { get; set; } = new Dictionary<string, int>()
        {
            { "donate-third", 3 },
            { "donate-first", 2 },
        };

        [Description("Лимиты по выдаче предметов для донатеров.")]
        public Dictionary<string, int> ItemLimits { get; set; } = new Dictionary<string, int>()
        {
            { "donate-third", 3 },
            { "donate-second", 3 },
        };
    }
}
