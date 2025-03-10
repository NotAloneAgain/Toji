﻿using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class DoorRestartSystemMalfunction : BaseMalfunction
    {
        public override string Name => "Перезапуск системы удаленного управления дверьми";

        public override int MinDuration => 8;

        public override int MaxDuration => 20;

        public override int Cooldown => 240;

        public override int Chance => 19;

        public override void Activate(int duration)
        {
            base.Activate(duration);

            if (Warhead.IsInProgress)
            {
                return;
            }

            var zone = SelectZone();

            Map.Broadcast(12, $"<color=#780000><b>Активирован {Name.ToLower()} {TranslateZone(zone)}, потребуется {GetSecondsString(duration)} для завершения</b></color>");

            foreach (Door door in Door.List)
            {
                if (door.IsLocked || !door.AllowsScp106)
                {
                    continue;
                }

                if (zone == ZoneType.Other && door.Zone == ZoneType.Surface || zone != ZoneType.Unspecified && door.Zone != zone)
                {
                    return;
                }

                door.IsOpen = false;

                door.Lock(duration, DoorLockType.SpecialDoorFeature);

                door.Room.TurnOffLights(0.15f);
            }
        }
    }
}
