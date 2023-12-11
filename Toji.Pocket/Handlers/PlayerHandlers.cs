using Exiled.API.Enums;
using Exiled.API.Features.Hazards;
using Exiled.Events.EventArgs.Player;
using Toji.ExiledAPI.Extensions;
using Toji.Pocket.API;

namespace Toji.Pocket.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnEnteringEnvironmentalHazard(EnteringEnvironmentalHazardEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid() || ev.Player.IsNoclipPermitted || ev.Hazard is not SinkholeHazard sinkhole || sinkhole.Room.Zone != ZoneType.LightContainment)
            {
                return;
            }

            ev.Player.RegisterEntering();
        }

        public void OnStayingEnvironmentalHazard(StayingOnEnvironmentalHazardEventArgs ev)
        {
            if (!ev.IsValid() || ev.Player.IsNoclipPermitted || ev.Hazard is not SinkholeHazard sinkhole || sinkhole.Room.Zone != ZoneType.LightContainment || ev.Player.GetStayingTime() < 6 || ev.Player.IsInPortal())
            {
                return;
            }

            ev.Player.PortalTeleport();
        }
    }
}
