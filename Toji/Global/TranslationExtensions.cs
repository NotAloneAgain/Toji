using Exiled.API.Enums;
using PlayerRoles;
using System.Collections.Generic;

namespace Toji.Global
{
    public static class TranslationExtensions
    {
        private static readonly IReadOnlyDictionary<RoleTypeId, string> _rolesTranslation;
        private static readonly IReadOnlyDictionary<RoleTypeId, string> _shortTranslation;

        static TranslationExtensions()
        {
            _rolesTranslation = new Dictionary<RoleTypeId, string>()
            {
                { RoleTypeId.None, "Пустая роль" },
                { RoleTypeId.Scp173, "SCP-173" },
                { RoleTypeId.ClassD, "Персонал Класса D" },
                { RoleTypeId.Scp106, "SCP-106" },
                { RoleTypeId.NtfSpecialist, "Девятихвостая Лиса — Специалист" },
                { RoleTypeId.Scp049, "SCP-049" },
                { RoleTypeId.Scientist, "Научный Сотрудник" },
                { RoleTypeId.Scp079, "SCP-079" },
                { RoleTypeId.ChaosConscript, "Повстанец Хаоса — Новобранец" },
                { RoleTypeId.Scp096, "SCP-096" },
                { RoleTypeId.Scp0492, "SCP-049-2" },
                { RoleTypeId.NtfSergeant, "Девятихвостая Лиса — Сержант" },
                { RoleTypeId.NtfCaptain, "Девятихвостая Лиса — Капитан" },
                { RoleTypeId.NtfPrivate, "Девятихвостая Лиса — Рядовой" },
                { RoleTypeId.Tutorial, "Обучение" },
                { RoleTypeId.FacilityGuard, "Охранник Комплекса" },
                { RoleTypeId.Scp939, "SCP-939" },
                { RoleTypeId.CustomRole, "Кастомная Роль" },
                { RoleTypeId.ChaosRifleman, "Повстанец Хаоса — Стрелок" },
                { RoleTypeId.ChaosMarauder, "Повстанец Хаоса — Мародёр" },
                { RoleTypeId.ChaosRepressor, "Повстанец Хаоса — Усмиритель" },
                { RoleTypeId.Overwatch, "Надзиратель" },
                { RoleTypeId.Filmmaker, "Контентмейкер" },
                { RoleTypeId.Scp3114, "SCP-3114" },
                { RoleTypeId.Spectator, "Наблюдатель" }
            };

            _shortTranslation = new Dictionary<RoleTypeId, string>()
            {
                { RoleTypeId.None, "Пусто" },
                { RoleTypeId.Scp173, "SCP-173" },
                { RoleTypeId.ClassD, "Класс D" },
                { RoleTypeId.Scp106, "SCP-106" },
                { RoleTypeId.NtfSpecialist, "Специалист МОГ" },
                { RoleTypeId.Scp049, "SCP-049" },
                { RoleTypeId.Scientist, "Ученый" },
                { RoleTypeId.Scp079, "SCP-079" },
                { RoleTypeId.ChaosConscript, "Новобранец ПХ" },
                { RoleTypeId.Scp096, "SCP-096" },
                { RoleTypeId.Scp0492, "SCP-049-2" },
                { RoleTypeId.NtfSergeant, "Сержант МОГ" },
                { RoleTypeId.NtfCaptain, "Капитан МОГ" },
                { RoleTypeId.NtfPrivate, "Рядовой МОГ" },
                { RoleTypeId.Tutorial, "Обучение" },
                { RoleTypeId.FacilityGuard, "Охранник" },
                { RoleTypeId.Scp939, "SCP-939" },
                { RoleTypeId.CustomRole, "Кастомка" },
                { RoleTypeId.ChaosRifleman, "Стрелок ПХ" },
                { RoleTypeId.ChaosMarauder, "Мародёр ПХ" },
                { RoleTypeId.ChaosRepressor, "Усмиритель ПХ" },
                { RoleTypeId.Overwatch, "Надзиратель" },
                { RoleTypeId.Filmmaker, "Контентмейкер" },
                { RoleTypeId.Scp3114, "SCP-3114" },
                { RoleTypeId.Spectator, "Наблюдатель" }
            };
        }

        public static string Translate(this RoleTypeId role) => _rolesTranslation.TryGetValue(role, out var value) ? value : role.ToString();

        public static string ShortTranslate(this RoleTypeId role) => _shortTranslation.TryGetValue(role, out var value) ? value : role.ToString();

        public static string TranslateZone(this ZoneType zone) => zone switch
        {
            ZoneType.LightContainment => "в лёгкой зоне содержания",
            ZoneType.HeavyContainment => "в тяжёлой зоне содержания",
            ZoneType.Entrance => "в офисной зоне",
            ZoneType.Surface => "на Поверхности",
            ZoneType.Other => "в неизвестной зоне",
            _ => "в неизвестности"
        };

        public static string Translate(this Team team) => team switch
        {
            Team.SCPs => "SCP-Объект",
            Team.FoundationForces => "МОГ",
            Team.ChaosInsurgency => "Хаос",
            Team.Scientists => "Научный персонал",
            Team.ClassD => "Персонал класса D",
            Team.Dead => "Мертвец",
            _ => "Неизвестно",
        };
    }
}
