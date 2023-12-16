using Exiled.Events.EventArgs.Player;
using System;
using Toji.Classes.API.Features.Abilities;
using Toji.Classes.API.Interfaces;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class ReflectionAbility : PassiveAbility, IHurtController
    {
        private bool _includeScp;
        private float _multiplayer;

        public ReflectionAbility(bool includeScp, float multiplayer)
        {
            _includeScp = includeScp;
            _multiplayer = multiplayer;
        }

        public override string Name => "Отражение урона";

        public override string Desc => $"Ты отражаешь {Math.Round(_multiplayer * 100, 1)}% урона в атакующего{(_includeScp ? string.Empty : ", если он не SCP-Объект")}";

        public void OnHurt(HurtingEventArgs ev)
        {
            if (!ev.IsNotSelfDamage() || ev.Attacker.IsGodModeEnabled || !ev.DamageHandler.Type.IsValid() || !IsEnabled)
            {
                return;
            }

            if (ev.Attacker.IsScp && !_includeScp)
            {
                return;
            }

            var amount = ev.Attacker.IsScp ? 50 : ev.Amount * _multiplayer;

            ev.Attacker.Hurt(ev.Player, amount, ev.DamageHandler.Type, default);
        }

        public override void Subscribe() { }

        public override void Unsubscribe() { }
    }
}
