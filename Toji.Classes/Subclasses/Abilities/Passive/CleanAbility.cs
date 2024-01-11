using Exiled.API.Enums;
using Exiled.API.Features.Hazards;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using System;
using System.Collections.Generic;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;

namespace Toji.Classes.Subclasses.Abilities.Passive
{
    public class CleanAbility : PassiveAbility
    {
        private Dictionary<string, DateTime> _times;

        public CleanAbility() => _times = new(10);

        public override string Name => "Уборка";

        public override string Desc => "Вы убираете отходы SCP-173, обнаруживая их под своими ногами.";

        public override void Subscribe()
        {
            Player.StayingOnEnvironmentalHazard += OnStayingOnEnvironmentalHazard;
            Player.EnteringEnvironmentalHazard += OnEnteringEnvironmentalHazard;
            Player.ExitingEnvironmentalHazard += OnExitingEnvironmentalHazard;
        }

        public override void Unsubscribe()
        {
            Player.ExitingEnvironmentalHazard -= OnExitingEnvironmentalHazard;
            Player.EnteringEnvironmentalHazard -= OnEnteringEnvironmentalHazard;
            Player.StayingOnEnvironmentalHazard -= OnStayingOnEnvironmentalHazard;
        }

        public void OnEnteringEnvironmentalHazard(EnteringEnvironmentalHazardEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid() || !Has(ev.Player) || ev.Hazard is not TantrumHazard)
            {
                return;
            }

            _times.Add(ev.Player.UserId, DateTime.Now);
        }

        public void OnStayingOnEnvironmentalHazard(StayingOnEnvironmentalHazardEventArgs ev)
        {
            if (!ev.IsValid() || !Has(ev.Player) || !_times.TryGetValue(ev.Player.UserId, out var time) || ev.Hazard is not TantrumHazard tantrum || (DateTime.Now - time).TotalSeconds > 6)
            {
                return;
            }

            tantrum.Destroy();

            foreach (var player in tantrum.AffectedPlayers)
            {
                player.DisableEffect(EffectType.Stained);
            }
        }

        public void OnExitingEnvironmentalHazard(ExitingEnvironmentalHazardEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid() || !Has(ev.Player) || !_times.ContainsKey(ev.Player.UserId) || ev.Hazard is not TantrumHazard)
            {
                return;
            }

            _times.Remove(ev.Player.UserId);
        }
    }
}
