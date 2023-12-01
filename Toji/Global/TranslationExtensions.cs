﻿using PlayerRoles;
using System.Collections.Generic;

namespace Toji.Global
{
    public static class TranslationExtensions
    {
        private static readonly IReadOnlyDictionary<RoleTypeId, string> _rolesTranslation;

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
            };
        }

        public static string Translate(this RoleTypeId role) => _rolesTranslation?[role] ?? role.ToString();
    }
}
