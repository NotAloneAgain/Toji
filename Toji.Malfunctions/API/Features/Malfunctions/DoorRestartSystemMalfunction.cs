using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class DoorRestartSystemMalfunction : BaseMalfunction
    {
        public override string Name => "Перезапуск системы удаленного управления дверьми";

        public override int MinDuration => 5;

        public override int MaxDuration => 15;

        public override int Cooldown => 220;

        public override int Chance => 19;

        public override void Activate(int duration)
        {
            var zone = SelectZone();

            Map.Broadcast(12, $"<color=#780000><b>Активирован {Name.ToLower()} {TranslateZone(zone)}, потребуется {GetSecondsString(duration)} для завершения</b></color>");

            foreach (Door door in Door.List)
            {
                if (door.IsLocked || !door.AllowsScp106 || door.Zone != zone && zone != ZoneType.Unspecified && (zone != ZoneType.Other || door.Zone == ZoneType.Surface))
                {
                    continue;
                }

                door.IsOpen = false;

                door.Lock(duration, DoorLockType.SpecialDoorFeature);

                door.Room.Blackout(0.15f);
            }
        }
    }
}
