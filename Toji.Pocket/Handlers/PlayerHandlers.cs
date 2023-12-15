using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Hazards;
using Exiled.Events.EventArgs.Player;
using System.Linq;
using Toji.ExiledAPI.Extensions;
using Toji.Pocket.API;
using UnityEngine;

namespace Toji.Pocket.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnEnteringEnvironmentalHazard(EnteringEnvironmentalHazardEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid() || ev.Player.IsNoclipPermitted || ev.Hazard is not SinkholeHazard sinkhole || GetClosest(sinkhole.Position).Zone != ZoneType.LightContainment)
            {
                return;
            }

            ev.Player.RegisterEntering();
        }

        public void OnStayingEnvironmentalHazard(StayingOnEnvironmentalHazardEventArgs ev)
        {
            if (!ev.IsValid() || ev.Player.IsNoclipPermitted || ev.Hazard is not SinkholeHazard sinkhole || GetClosest(sinkhole.Position).Zone != ZoneType.LightContainment || ev.Player.GetStayingTime() < 6 || ev.Player.IsInPortal())
            {
                return;
            }

            ev.Player.PortalTeleport();
        }

        private Room GetClosest(Vector3 vector)
        {
            var room = Room.List.OrderBy(x => Vector3.Distance(vector, x.Position)).FirstOrDefault();

            if (room == null)
            {
                return Room.Get(RoomType.EzCrossing);
            }

            return room;
        }
    }
}
