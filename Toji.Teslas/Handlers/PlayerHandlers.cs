using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace Toji.Teslas.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (!ev.IsAllowed || ev.Player.IsHost || ev.Player.IsNPC || !Warhead.IsInProgress && ev.Player.Role.Team != Team.FoundationForces && ev.Player.Role.Type != RoleTypeId.Scp079 && !ev.Tesla.Room.AreLightsOff)
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
