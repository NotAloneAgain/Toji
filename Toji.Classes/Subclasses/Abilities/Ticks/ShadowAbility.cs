using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
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

        public override Func<Player, bool> PlayerCondition => (Player ply) => base.PlayerCondition(ply) && ply.IsAlive;

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

            if (player.Position.y is >= 970 and <= 1050)
            {
                player.Hurt(84);

                return;
            }

            var room = player.CurrentRoom;

            if (room == null)
            {
                return;
            }

            if (room.Type == RoomType.Surface)
            {
                player.Hurt(84);

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
                if (ply == null || ply.IsHost || ply.CurrentRoom != room || ply.IsScp)
                {
                    continue;
                }

                if (ply.CurrentItem != null && (ply.CurrentItem.Base is ILightEmittingItem light && light.IsEmittingLight || ply.CurrentItem is Flashlight flash && flash.IsEmittingLight))
                {
                    continue;
                }

                float percent = 6.5f - Mathf.Clamp(Vector3.Distance(ply.Position, player.Position), 1, 5);

                if (ply.CurrentItem is Firearm firearm && firearm.FlashlightEnabled)
                {
                    percent /= 4;
                }

                ply.Hurt(ply.MaxHealth * (percent / 100f));
            }
        }
    }
}
