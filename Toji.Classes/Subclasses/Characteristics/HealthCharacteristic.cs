using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features.Characteristics;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class HealthCharacteristic(float value) : Characteristic<float>(value)
    {
        private static Dictionary<RoleTypeId, int> _defaultValues;

        static HealthCharacteristic() => _defaultValues = new Dictionary<RoleTypeId, int>()
            {
                { RoleTypeId.None, 0 },
                { RoleTypeId.Scp173, 4000 },
                { RoleTypeId.Scp106, 2200 },
                { RoleTypeId.Scp3114, 1250 },
                { RoleTypeId.Scp049, 2200 },
                { RoleTypeId.Scp0492, 400 },
                { RoleTypeId.Scp096, 2500 },
                { RoleTypeId.Scp939, 2400 },
                { RoleTypeId.Scp079, int.MaxValue },
                { RoleTypeId.ChaosConscript, 100 },
                { RoleTypeId.ChaosMarauder, 100 },
                { RoleTypeId.ChaosRepressor, 100 },
                { RoleTypeId.ChaosRifleman, 100 },
                { RoleTypeId.Scientist, 100 },
                { RoleTypeId.FacilityGuard, 100 },
                { RoleTypeId.NtfCaptain, 100 },
                { RoleTypeId.NtfSergeant, 100 },
                { RoleTypeId.NtfPrivate, 100 },
                { RoleTypeId.NtfSpecialist, 100 },
                { RoleTypeId.Overwatch, 100 },
                { RoleTypeId.Filmmaker, 100 },
                { RoleTypeId.Tutorial, 100 },
                { RoleTypeId.Spectator, 100 },
                { RoleTypeId.CustomRole, 100 },
            };

        public override string Name => "Измененное количество здоровья";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.MaxHealth = Value;
            player.Health = Value;
        }

        public override void OnDisabled(Player player)
        {
            player.MaxHealth = GetDefaultValue(player?.Role ?? RoleTypeId.None);
            player.Health = GetDefaultValue(player?.Role ?? RoleTypeId.None);

            base.OnDisabled(player);
        }

        public override string GetDesc(Player player = null)
        {
            var defaultValue = GetDefaultValue(player?.Role ?? RoleTypeId.None);

            return Value > defaultValue ? "Ваше количество здоровья больше стандартного" : "Ваше количество здоровья меньше стандартного";
        }

        private int GetDefaultValue(RoleTypeId role = RoleTypeId.None) => _defaultValues.TryGetValue(role, out int value) ? value : 100;
    }
}
