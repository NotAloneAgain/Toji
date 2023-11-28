using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class ZoneLightMalfunction : BaseMalfunction
    {
        public override string Name => "Отключение электричества в зоне";

        public override int Chance => 41;

        public override int Duration => Random.Range(20, 80);

        public override void Activate(int duration)
        {
            base.Activate(duration);

            var zone = SelectZone();

            Map.TurnOffAllLights(duration, zone);
        }

        private protected override void Subscribe() { }

        private protected override void Unsubscribe() { }

        private ZoneType SelectZone() => Random.Range(0, 101) switch
        {
            >= 80 => ZoneType.Surface,
            >= 60 => ZoneType.Entrance,
            >= 40 => ZoneType.HeavyContainment,
            >= 20 => ZoneType.Unspecified,
            _ => ZoneType.LightContainment
        };
    }
}
