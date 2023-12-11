using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class ZoneLightMalfunction : BaseMalfunction
    {
        public override string Name => "Отключение электричества";

        public override int Chance => 30;

        public override int MinDuration => 20;

        public override int MaxDuration => 80;

        public override int Cooldown => 110;

        public override void Activate(int duration)
        {
            var zone = SelectZone();

            Map.Broadcast(12, $"<color=#780000><b>Внимание всем!\nПроизошло {Name.ToLower()} {TranslateZone(zone)}, исправление займет {GetSecondsString(duration)}</b></color>");

            Map.TurnOffAllLights(duration, zone);
        }

        public override void Subscribe() { }

        public override void Unsubscribe() { }

        private ZoneType SelectZone() => Random.Range(0, 101) switch
        {
            >= 80 => ZoneType.Surface,
            >= 60 => ZoneType.Entrance,
            >= 40 => ZoneType.HeavyContainment,
            >= 20 => ZoneType.Other,
            >= 10 => ZoneType.Unspecified,
            _ => ZoneType.LightContainment
        };
    }
}
