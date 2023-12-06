using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Features;
using Toji.Global;
using UnityEngine;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class HurtMultiplayerCharacteristic : Characteristic<float>
    {
        private HashSet<RoleTypeId> _roles;

        public HurtMultiplayerCharacteristic(float value) : base(value)
        {
            _roles = new HashSet<RoleTypeId>(0);
        }

        public HurtMultiplayerCharacteristic(float value, IEnumerable<RoleTypeId> roles) : this(value)
        {
            _roles = roles.ToHashSet();
        }

        public override string Name => "Модификатор урона";

        public override string GetDesc(Player player = null)
        {
            var value = Value - 1;

            string roles = "для ";

            if (_roles.Any())
            {
                foreach (var role in _roles)
                {
                    roles += $"{role.ShortTranslate()} ";
                }
            }
            else
            {
                roles += "всех";
            }

            return value > 0 ? $"Входящий урон повышен на {value * 100}% {roles}" : $"Входящий урон понижен на {Mathf.Abs(value * 100)}% {roles}";
        }
    }
}
