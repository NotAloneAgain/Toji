using Exiled.API.Enums;
using Exiled.API.Features;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class InvisibleAbility : PassiveAbility
    {
        public override string Name => "Невидимость";

        public override string Desc => "Никто и никогда вас не увидит";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.EnableEffect(EffectType.Invisible);
        }

        public override void OnDisabled(Player player)
        {
            player.DisableEffect(EffectType.Invisible);

            base.OnDisabled(player);
        }

        public override void Subscribe() { }

        public override void Unsubscribe() { }
    }
}
