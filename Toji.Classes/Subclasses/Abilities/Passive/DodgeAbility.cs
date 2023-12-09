using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class DodgeAbility : ChanceAbility
    {
        private int _fullyChance;
        private float _defaultHurt;

        public DodgeAbility(int chance, int fullyChance, float defaultHurt) : base(chance)
        {
            _fullyChance = fullyChance;
            _defaultHurt = defaultHurt;
        }

        public override string Name => "Уклонение";

        public override string Desc => $"Ты можешь уклониться от атаки с шансом {Chance}%, смягчив на {100 - _defaultHurt * 100}% ({100 - _fullyChance}%) или вовсе не получив ({_fullyChance}%) урон";

        public override void Subscribe() => Player.Hurting += OnHurting;

        public override void Unsubscribe() => Player.Hurting -= OnHurting;

        private void OnHurting(HurtingEventArgs ev)
        {
            if (!ev.IsValid() || !ev.IsAllowed || !Has(ev.Player) || ev.DamageHandler.Type == DamageType.Warhead || !GetRandom())
            {
                return;
            }

            if (GetRandom(_fullyChance) && ev.DamageHandler.Type.IsValid())
            {
                ev.Amount = 0;
            }
            else
            {
                ev.Amount *= _defaultHurt;
            }
        }
    }
}
