using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class ZoneLightMalfunction : BaseMalfunction
    {
        public override string Name => "Отключение электричества";

        public override int Chance => 30;

        public override int MinDuration => 20;

        public override int MaxDuration => 80;

        public override int Cooldown => 190;

        public override void Activate(int duration)
        {
            base.Activate(duration);

            var zone = SelectZone();

            if (zone == ZoneType.LightContainment && Map.IsLczDecontaminated)
            {
                zone = ZoneType.HeavyContainment;
            }

            Map.Broadcast(12, $"<color=#780000><b>Внимание всем!\nПроизошло {Name.ToLower()} {TranslateZone(zone)}, исправление займет {GetSecondsString(duration)}</b></color>");

            if (zone == ZoneType.Other)
            {
                Map.TurnOffAllLights(duration, new List<ZoneType>() { ZoneType.LightContainment, ZoneType.HeavyContainment, ZoneType.Entrance });
            }

            Map.TurnOffAllLights(duration, zone);
        }
    }
}
