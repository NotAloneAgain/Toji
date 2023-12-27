using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using Toji.ExiledAPI.Extensions;

namespace Toji.Teslas.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (!ev.IsAllowed || !ev.IsValid() || !Warhead.IsInProgress && ev.Player.Role.Team is not Team.FoundationForces and not Team.Flamingos and not Team.OtherAlive && ev.Player.Role.Type != RoleTypeId.Scp079 && !ev.Tesla.Room.AreLightsOff && !ev.Player.HasEffect<Invisible>())
            {
                return;
            }

            ev.IsAllowed = false;
            ev.IsInIdleRange = false;
            ev.IsInHurtingRange = false;
            ev.IsTriggerable = false;
        }
    }
}
