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

        public override string Desc => "Твоя аура поглащает свет в комнате и наносит урон тем, кто не достал фонари";

        public override Func<Player, bool> PlayerCondition => (Player ply) => ply != null && ply.IsAlive;

        public override void Iteration(Player player)
        {
            var room = player.CurrentRoom;

            if (room.Type == RoomType.Surface)
            {
                player.Hurt(100, DamageType.Bleeding);

                return;
            }

            if (_previousRoom != room)
            {
                _previousRoom.AreLightsOff = false;

                room.TurnOffLights(1.1f);
            }
            else
            {
                room.AreLightsOff = true;
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
