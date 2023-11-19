﻿using Exiled.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;

namespace Toji.ScpSwap.Configs
{
    public sealed class Config : IConfig
    {
        [Description("Включено ли?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Включить ли отладку?")]
        public bool Debug { get; set; } = false;

        [Description("Предотвращать ли множественный свап?")]
        public bool PreventMultipleSwaps { get; set; } = true;

        [Description("Текст подсказки с информацией по плагину.")]
        public string InfoText { get; set; } = "<line-height=95%><size=95%><voffset=-20em><color=#E32636>Ты можешь сменить свой класс с помощью команды <b>.force</b>." +
            "\nНа это у тебя есть <b>{0}</b> секунд.</color></size></voffset>";

        [Description("Длительность подсказки.")]
        public float InfoDuration { get; set; } = 12;

        [Description("Длительность возможности перехода")]
        public ushort SwapDuration { get; set; } = 90;

        [Description("SCP которым разрешено менять роль.")]
        public List<RoleTypeId> AllowedScps { get; set; } = new()
        {
            RoleTypeId.Scp096,
            RoleTypeId.Scp049,
            RoleTypeId.Scp173,
            RoleTypeId.Scp939,
            RoleTypeId.Scp106,
            RoleTypeId.Scp079,
            RoleTypeId.Scp3114
        };

        [Description("Количество слотов за данного SCP.")]
        public Dictionary<RoleTypeId, int> Slots { get; set; } = new()
        {
            { RoleTypeId.Scp096, 1 },
            { RoleTypeId.Scp049, 1 },
            { RoleTypeId.Scp173, 1 },
            { RoleTypeId.Scp939, 2 },
            { RoleTypeId.Scp106, 1 },
            { RoleTypeId.Scp079, 1 },
            { RoleTypeId.Scp3114, 1 }
        };
    }
}
