﻿using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Doors;
using System;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Scp575
{
    public class FirstShadowRazeAbility : CooldownAbility
    {
        public FirstShadowRazeAbility(uint cooldown) : base(cooldown)
        {
        }

        public override string Name => "Shadowraze-ближний";

        public override string Desc => "Моментально атакуете всех людей в комнате, блокируете и закрываете дверь в комнату";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (Round.ElapsedTime.TotalMinutes < 1)
            {
                result = "Способность разблокируется через 1 минуту после начала раунда!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var room = player.CurrentRoom;

            if (room.Type == RoomType.Surface)
            {
                result = "Ты не можешь активировать способности на Поверхности!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            room.TurnOffLights(14);

            foreach (var door in room.Doors)
            {
                if (door.Is<BreakableDoor>(out var breakable) && (breakable.IsLocked || !breakable.AllowsScp106))
                {
                    continue;
                }

                if (!door.IsGate && !door.IsElevator && door.IsOpen)
                {
                    door.IsOpen = false;
                }

                if (!door.IsLocked)
                {
                    door.Lock(14, DoorLockType.Lockdown079);
                }
            }

            foreach (var ply in room.Players)
            {
                if (ply.IsHost || ply.IsDead || ply.IsScp)
                {
                    continue;
                }

                new CustomDamageHandler(ply, player, 10, DamageType.Scp106).ApplyDamage(ply);
            }

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
