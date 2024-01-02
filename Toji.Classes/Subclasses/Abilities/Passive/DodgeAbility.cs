using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class DodgeAbility(int chance, int fullyChance, float defaultHurt) : ChanceAbility(chance)
    {
        public override string Name => "Уклонение";

        public override string Desc => $"Ты можешь уклониться от атаки с шансом {Chance}%, смягчив на {100 - defaultHurt * 100}% ({100 - fullyChance}%) или вовсе не получив ({fullyChance}%) урон";

        public override void Subscribe() => Player.Hurting += OnHurting;

        public override void Unsubscribe() => Player.Hurting -= OnHurting;

        private void OnHurting(HurtingEventArgs ev)
        {
            if (!ev.IsValid() || !IsEnabled || !ev.IsAllowed || !Has(ev.Player) || !ev.DamageHandler.Type.IsValid() || !GetRandom() || ev.Amount <= 0)
            {
                return;
            }

            if (GetRandom(fullyChance))
            {
                ev.Amount = 0;
            }
            else
            {
                ev.Amount *= defaultHurt;
            }
        }
    }
}
