using Exiled.API.Features;
using System;
using Toji.Classes.API.Features.Abilities;
using Toji.Classes.Subclasses.Abilities.Enums;
using UnityEngine;

namespace Toji.Classes.Subclasses.Abilities.Ticks
{
    public class HealAura : TickAbility
    {
        private ushort _ticks;
        private float _health;
        private float _distance;
        private HealAuraTargets _targets;

        public HealAura(ushort ticks, float health, float distance, HealAuraTargets targets)
        {
            _ticks = ticks;
            _health = health;
            _distance = distance;
            _targets = targets;
        }

        public override string Name => _health > 0 ? "Аура лечения" : "Аура смерти";

        public override string Desc => $"Аура, {(_health > 0 ? "исцеляющая" : "убивающая")} {ParseTargets()} с силой {Math.Round(_health) * _ticks} здоровья/секунда";

        public override ushort TicksPerSecond => _ticks;

        public override Func<Player, bool> PlayerCondition => (Player ply) => ply != null && ply.IsAlive;

        public override void Iteration(Player player)
        {
            foreach (var ply in Player.List)
            {
                if (Vector3.Distance(ply.Position, player.Position) > _distance || !CheckTarget(ply, player))
                {
                    continue;
                }

                ply.Heal(_health);
            }
        }

        private bool CheckTarget(Player player, Player owner)
            => _targets == HealAuraTargets.All
            || _targets == HealAuraTargets.Scps && player.IsScp
            || _targets == HealAuraTargets.Humans && player.IsHuman
            || _targets == HealAuraTargets.Side && player.Role.Side == owner.Role.Side
            || _targets == HealAuraTargets.Team && player.Role.Team == owner.Role.Team
            || _targets == HealAuraTargets.Role && player.Role.Type == owner.Role.Type;

        private string ParseTargets() => _targets switch
        {
            HealAuraTargets.All => "всех",
            HealAuraTargets.Scps => "SCP-Объекты",
            HealAuraTargets.Humans => "людей",
            HealAuraTargets.Side => "игроков вашей фракции",
            HealAuraTargets.Team => "игроков вашей команды",
            HealAuraTargets.Role => "игроков вашей роли",
            _ => "???"
        };
    }
}
