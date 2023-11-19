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
            "genshin",
            "don3",
            "don2",
            "don1",
        };

        [Description("Лимиты по форсу для донатеров.")]
        public Dictionary<string, int> ForceLimits { get; set; } = new Dictionary<string, int>()
        {
            { "don3", 3 },
            { "don1", 2 },
        };

        [Description("Лимиты по выдаче предметов для донатеров.")]
        public Dictionary<string, int> ItemLimits { get; set; } = new Dictionary<string, int>()
        {
            { "genshin", 3 },
            { "don3", 3 },
            { "don1", 3 },
        };
    }
}
