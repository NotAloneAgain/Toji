using Exiled.API.Enums;
using Exiled.API.Features;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class HealAbility(uint cooldown) : CooldownAbility(cooldown)
    {
        private float _distance = 4;
        private float _stamina = 0.15f;
        private int _health = 40;

        public HealAbility(uint cooldown, float distance, float stamina, int health) : this(cooldown)
        {
            _distance = distance;
            _stamina = stamina;
            _health = health;
        }

        public override string Name => "Лечение";

        public override string Desc => "Ты можешь подлатать союзника";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            if (player.IsCuffed)
            {
                result = $"Ты не можешь лечить, будучи связанным.";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            var target = player.GetFromView(_distance);

            if (target == null || target.IsHost || target.IsNPC || target.IsTutorial)
            {
                result = "Не удалось получить цель!";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            if (target.Health == target.MaxHealth)
            {
                result = "У него и так полное здоровье!";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            if (target.Role.Side != player.Role.Side)
            {
                result = "Ты не можешь лечить врагов!";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            target.Heal(_health, false);
            target.DisableEffect(EffectType.Bleeding);
            target.Stamina += _stamina;

            result = $"Ты успешно подлатал {target.CustomName}";

            AddUse(player, System.DateTime.Now, true, result);

            return true;
        }
    }
}
