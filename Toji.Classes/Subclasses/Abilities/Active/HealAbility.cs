using Exiled.API.Enums;
using Exiled.API.Features;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class HealAbility : CooldownAbility
    {
        public HealAbility(uint cooldown) : base(cooldown) { }

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

            var target = player.GetFromView(4);

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

            target.Heal(40, false);
            target.DisableEffect(EffectType.Bleeding);
            target.Stamina += 0.15f;

            result = $"Ты успешно подлатал {target.CustomName}";

            AddUse(player, System.DateTime.Now, true, result);

            return true;
        }
    }
}
