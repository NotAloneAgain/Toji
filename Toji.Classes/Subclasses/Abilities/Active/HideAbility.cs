using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class HideAbility : CooldownAbility
    {
        private float _duration;

        public HideAbility(uint cooldown, float duration) : base(cooldown)
        {
            _duration = duration;
        }

        public override string Name => "Скрыться";

        public override string Desc => "Вы станете невидимым для других игроков";

        public override void Subscribe()
        {
            base.Subscribe();

            Exiled.Events.Handlers.Player.Shot += OnShot;
        }

        public override void Unsubscribe()
        {
            Exiled.Events.Handlers.Player.Shot -= OnShot;

            base.Unsubscribe();
        }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (player.IsCuffed)
            {
                result = "Ты не можешь скрыться, будучи связанным.";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (player.HasEffect<Invisible>())
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            player.EnableEffect(EffectType.Invisible, _duration);

            AddUse(player, DateTime.Now, true, result);

            return true;
        }

        private void OnShot(ShotEventArgs ev)
        {
            if (!ev.CanHurt || ev.Target == null || !Has(ev.Player))
            {
                return;
            }

            ev.Player.DisableEffect(EffectType.Invisible);
        }
    }
}
