using Exiled.API.Features;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class InfinityStaminaAbility : PassiveAbility
    {
        public override string Name => "Бесконечная выносливость";

        public override string Desc => "Ты можешь бегать бесконечно";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.IsUsingStamina = false;
        }

        public override void OnDisabled(Player player)
        {
            player.IsUsingStamina = true;

            base.OnDisabled(player);
        }

        public override void Subscribe() { }

        public override void Unsubscribe() { }
    }
}
