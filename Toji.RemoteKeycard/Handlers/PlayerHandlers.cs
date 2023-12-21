using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Linq;
using Toji.ExiledAPI.Extensions;
using Toji.RemoteKeycard.API;
using Toji.RemoteKeycard.API.Enums;
using Toji.RemoteKeycard.API.Features;

namespace Toji.RemoteKeycard.Handlers
{
    internal sealed class PlayerHandlers
    {
        public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!ev.IsValid() || ev.Door.Is(out BreakableDoor breakable) && breakable.IsDestroyed || ev.Door.IsMoving)
            {
                return;
            }

            ev.IsAllowed = BaseDoorProcessor.Get(ev.Player.IsScp ? DoorProcessorType.Scp : DoorProcessorType.Human).ProcessDoor(ev.Door, ev.Player);
        }

        public void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.IsAllowed || !ev.IsValid())
            {
                return;
            }

            var hasAccess = ev.Player.CheckPermissions(ev.Chamber.RequiredPermissions);
            var hasCheckpoints = ev.Player.CheckPermissions(Interactables.Interobjects.DoorUtils.KeycardPermissions.Checkpoints);
            var hasContainment = ev.Player.CheckPermissions(Interactables.Interobjects.DoorUtils.KeycardPermissions.ContainmentLevelTwo);

            ev.IsAllowed = hasAccess || ev.Locker.Loot.All(x => !x.TargetItem.IsWeapon()) && hasCheckpoints && hasContainment;
        }

        public void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (!ev.IsValid())
            {
                return;
            }

            if (Round.ElapsedTime.TotalMinutes < 4)
            {
                ev.IsAllowed = false;

                return;
            }

            if (ev.Player.HasEffect<AmnesiaItems>() || ev.IsAllowed)
            {
                return;
            }

            ev.IsAllowed = ev.Player.CheckPermissions(ev.Generator.Base._requiredPermission);
        }
    }
}
