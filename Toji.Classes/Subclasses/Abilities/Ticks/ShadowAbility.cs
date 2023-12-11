using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items;
using System;
using Toji.Classes.API.Features.Abilities;
using UnityEngine;

namespace Toji.Classes.Subclasses.Abilities.Ticks
{
    public class ShadowAbility : TickAbility
    {
        private Room _previousRoom;

        public override string Name => "Поглощение света";

        public override string Desc => "Твоя аура поглощает свет в комнате и наносит урон тем, кто не достал фонари";

        public override Func<Player, bool> PlayerCondition => (Player ply) => ply != null && ply.IsAlive;

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            StartCoroutine();
        }

        public override void OnDisabled(Player player)
        {
            StopCoroutine();

            base.OnDisabled(player);
        }

        public override void Iteration(Player player)
        {
            if (player == null)
            {
                return;
            }

            var room = player.CurrentRoom;

            if (room == null)
            {
                return;
            }

            if (room.Type == RoomType.Surface)
            {
                player.Hurt(100, DamageType.Bleeding);

                return;
            }

            if (_previousRoom != room && _previousRoom != null)
            {
                _previousRoom.AreLightsOff = true;
            }
            else if (!room.AreLightsOff)
            {
                room.AreLightsOff = false;
            }

            _previousRoom = room;

            foreach (var ply in Player.List)
            {
                if (ply.CurrentRoom != room || ply.IsScp)
                {
                    continue;
                }

                if (ply.CurrentItem is ILightEmittingItem light && light.IsEmittingLight)
                {
                    return;
                }

                float percent = 6 - Mathf.Clamp(Vector3.Distance(ply.Position, player.Position), 1, 5);

                if (ply.CurrentItem is Firearm firearm && firearm.FlashlightEnabled)
                {
                    percent /= 4;
                }

                ply.Hurt(player, ply.MaxHealth * (percent / 100), DamageType.Crushed, default);
            }
        }
    }
}
