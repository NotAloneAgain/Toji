using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using System;
using Toji.Classes.API.Features.Abilities;
using Toji.Classes.API.Interfaces;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class ReflectionAbility(bool includeScp, float multiplayer) : PassiveAbility, IHurtController
    {
        public override string Name => "Отражение урона";

        public override string Desc => $"Ты отражаешь {Math.Round(multiplayer * 100, 1)}% урона в атакующего{(includeScp ? string.Empty : ", если он не SCP-Объект")}";

        public void OnHurt(HurtingEventArgs ev)
        {
            if (!ev.IsNotSelfDamage() || !IsEnabled || ev.Attacker.IsGodModeEnabled || !ev.DamageHandler.Type.IsValid() || ev.DamageHandler.Type == DamageType.Explosion || ev.Amount <= 0)
            {
                return;
            }

            if (ev.Attacker.IsScp && !includeScp)
            {
                return;
            }

            var amount = ev.Attacker.IsScp ? 50 : ev.Amount * multiplayer;

            ev.Attacker.Hurt(ev.Player, amount, ev.DamageHandler.Type, default);
        }

        public override void Subscribe() { }

        public override void Unsubscribe() { }
    }
}
