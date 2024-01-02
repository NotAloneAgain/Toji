using System.Collections.Generic;
using Toji.ExiledAPI.Configs;

namespace Toji.Hitmarker.Configs
{
    public sealed class Config : DefaultConfig
    {
        public List<string> DeathTexts { get; set; } = [

            "Убит!",
            "Подбит!",
            "Есть пробитие!",
            "Удачи на том свете!",
            "Славянский зажим яйцами!",
            "Твоя ма... Ваша ма... Прости, я так не могу...",
        ];
    }
}
