using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Scp575
{
    public class SecondShadowRazeAbility : CooldownAbility
    {
        public SecondShadowRazeAbility(uint cooldown) : base(cooldown)
        {
        }

        public override string Name => "Shadowraze-средний";

        public override string Desc => "Моментально атакуете всех людей в зоне, выключаете в ней свет и блокируете двери";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (Round.ElapsedTime.TotalMinutes < 3)
            {
                result = "Способность разблокируется через 3 минуты после начала раунда!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var zone = player.CurrentRoom.Zone;

            if (zone == ZoneType.Surface)
            {
                result = "Ты не можешь активировать ярость на Поверхности!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            Map.TurnOffAllLights(30, zone);

            foreach (var door in Door.List)
            {
                if (door.DoorLockType == DoorLockType.AdminCommand || door.Is<BreakableDoor>(out var breakable) && breakable.IsLocked && !breakable.AllowsScp106 || door.Zone != zone)
                {
                    continue;
                }

                door.IsOpen = false;

                if (!door.IsLocked)
                {
                    door.Lock(15, DoorLockType.AdminCommand);
                }
            }

            foreach (var ply in Player.List)
            {
                if (ply.Zone != zone || ply.IsHost || ply.IsDead || ply.IsScp)
                {
                    continue;
                }

                ply.Hurt(player, 15, DamageType.Crushed, default);
            }

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
