using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Doors;
using System;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Scp575
{
    public class FirstShadowRazeAbility(uint cooldown) : CooldownAbility(cooldown)
    {
        public override string Name => "Shadowraze-ближний";

        public override string Desc => "Моментально атакуете всех людей в комнате, блокируете и закрываете дверь в комнату";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
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

            room.TurnOffLights(15);

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
                    door.Lock(15, DoorLockType.Lockdown079);
                }
            }

            foreach (var ply in room.Players)
            {
                if (ply == null || ply.IsHost || ply.IsDead || ply.IsScp)
                {
                    continue;
                }

                ply.Hurt(10);
            }

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
