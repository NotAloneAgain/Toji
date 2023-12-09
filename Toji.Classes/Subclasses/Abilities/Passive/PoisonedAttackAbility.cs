using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using Toji.Classes.API.Features.Abilities;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class PoisonedAttackAbility : PassiveAbility, IDamageController
    {
        public override string Name => "Отравленные атаки";

        public override string Desc => "Ваши атаки отравляют и заставлять гнить жертв";

        public override void Subscribe() { }

        public override void Unsubscribe() { }

        public void OnDamage(HurtingEventArgs ev)
        {
            ev.Player.EnableEffect(EffectType.Traumatized);
            ev.Player.EnableEffect(EffectType.SinkHole, 4, true);
            ev.Player.EnableEffect(EffectType.Poisoned, 5, true);
            ev.Player.Stamina -= 0.02f;
        }
    }
}
