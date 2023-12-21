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
            if (!ev.IsValid() || ev.IsAllowed && ev.Player.IsHuman || ev.Door.Is(out BreakableDoor breakable) && breakable.IsDestroyed || ev.Door.IsMoving || ev.Player.HasEffect<AmnesiaItems>())
            {
                return;
            }

            if (ev.Player.IsScp && ev.Door.IsLocked && ev.Player.Role.Type != RoleTypeId.Scp079)
            {
                ev.IsAllowed = ev.Door.DoorLockType is DoorLockType.Regular079 or DoorLockType.Lockdown079;

                return;
            }

            if (ev.IsAllowed || ev.Door.IsLocked || !ev.Door.AllowsScp106 || !ev.Door.IsKeycardDoor)
            {
                return;
            }

            ev.IsAllowed = (ev.Door.IsCheckpoint || ev.Door.Type is DoorType.CheckpointArmoryA or DoorType.CheckpointArmoryB) ? ev.Player.CheckPermissions(Interactables.Interobjects.DoorUtils.KeycardPermissions.Checkpoints) : ev.Player.CheckPermissions(ev.Door.RequiredPermissions.RequiredPermissions);
        }

        /*public void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!ev.IsValid() || ev.Door.Is(out BreakableDoor breakable) && breakable.IsDestroyed || ev.Door.IsMoving)
            {
                return;
            }

            ev.IsAllowed = (ev.Player.IsScp ? (BaseDoorProcessor)_scp : _human).ProcessDoor(ev.Door, ev.Player);
        }*/

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
