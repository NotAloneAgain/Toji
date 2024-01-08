using System.Collections.Generic;
using Toji.ExiledAPI.Configs;

namespace Toji.NicknameFilters.Configs
{
    public sealed class Config : DefaultConfig
    {
        public List<string> BadWordsKick { get; set; } = [

            "Пидор",
            "Пидорас",
            "Гей",
            "Гитлер",
            "Китлер",
            "Хуесос",
            "Членосос",
            "Выблядок",
            "Ебарь",
            "Ебатель",
            "хай гитлер",
            "MIDNIGHT",
            "midnight.im",
            "хуеглот",
            "мефедрон",
            "Мразь",
            "твоя мама",
            "нахуй"
        ];

        public List<string> BadWordsReplacer { get; set; } = [

            "Человек яйца",
            "Человек-яйца",
            "Рей",
            "Грей",
            "Ray Grey",
            "Владелец",
            "Админ",
            "Гомодрил",
            "pidor"
        ];

        public List<string> Ads { get; set; } = [

            "#fydne",
            "#Runic Library",
            "#Runic",
            "#GRAND-RUST",
            "#Just Rust",
            "#RUST",
            "#grand rust",
            "#boloto rust",
            "boloto rust",
            "#brorust",
            "EasyDrop",
            "https:",
            "rustbox.io",
            "DangerZone",
            "#magicrust",
            "#Zone",
            "#runiclibrary",
            "#bs project",
            "#bloodrust",
            "#Хлеб",
            "#kyle",
            "#kyles",
            "#kyleproject",
            "#project",
            "@gmail.com",
            "@mail.ru",
            "csmoney",
            "ggdrop",
            "fireskins",
            "csgomoney",
            "zozo.gg",
            "grand-rust",
            "csgo.run",
            "pvpro.com",
            "cs.money",
            "magicdrop.ru",
            "gocs.com",
            "boomdrop.ru",
            "force-drop.net",
            "forcedrop.gg",
            "gamehag.com",
            "betrefs.com",
            "gg.bet",
            "mannco.trade",
            "сsgetto.bet",
            "csdrop",
            "csfail",
            "csgowin.ru"
        ];
    }
}
