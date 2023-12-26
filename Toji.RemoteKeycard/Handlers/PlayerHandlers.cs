using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using Toji.ExiledAPI.Extensions;
using Toji.Global;
using Toji.RemoteKeycard.API;
using Toji.RemoteKeycard.API.Features;
using Toji.RemoteKeycard.Processors;

namespace Toji.RemoteKeycard.Handlers
{
    internal sealed class PlayerHandlers
    {
        private readonly HumanDoorProcessor _human;
        private readonly ScpDoorProcessor _scp;

        public PlayerHandlers()
        {
            _human = new();
            _scp = new();
        }

        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!ev.IsValid() || ev.Door.Is(out BreakableDoor breakable) && breakable.IsDestroyed || ev.Door.IsMoving)
            {
                return;
            }

            ev.IsAllowed = (ev.Player.IsScp ? (BaseDoorProcessor)_scp : _human).ProcessDoor(ev.Door, ev.Player);
        }

        public void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.IsAllowed || !ev.IsValid())
            {
                return;
            }

            ev.IsAllowed = ev.Chamber.CanInteract && ev.Player.CheckPermissions(ev.Locker, ev.Chamber);
        }

        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
            => ev.IsAllowed = ev.IsValid() && (ev.Player.IsBypassModeEnabled || Round.ElapsedTime.TotalMinutes >= 4 && ev.Player.CheckPermissions(ev.Generator.KeycardPermissions));
    }
}
