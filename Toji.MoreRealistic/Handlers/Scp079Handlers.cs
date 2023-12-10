using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp079;
using Toji.ExiledAPI.Extensions;

namespace Toji.Hitmarker.Handlers
{
    internal sealed class Scp079Handlers
    {
        public void OnTriggeringDoor(TriggeringDoorEventArgs ev)
        {
            if (!ev.IsValid() || ev.IsAllowed || ev.Door == null)
            {
                return;
            }

            ev.Door.Lock(ev.Door.IsGate ? 2 : 0.6f, DoorLockType.Regular079);
        }

        public void OnRoomBlackout(RoomBlackoutEventArgs ev)
        {
            if (!ev.IsValid() || !ev.IsAllowed || ev.Room.AreLightsOff)
            {
                return;
            }

            if (ev.Room.Zone == ZoneType.LightContainment && Map.IsLczDecontaminated)
            {
                ev.IsAllowed = false;

                return;
            }

            if (ev.Room.Type == RoomType.Surface)
            {
                ev.Cooldown += 10;
                ev.BlackoutDuration -= 5;
                ev.AuxiliaryPowerCost *= 1.1f;
            }
        }

        public void OnZoneBlackout(ZoneBlackoutEventArgs ev)
        {
            if (!ev.IsValid() || !ev.IsAllowed || ev.Zone != ZoneType.LightContainment || !Map.IsLczDecontaminated)
            {
                return;
            }

            ev.IsAllowed = false;
        }
    }
}
