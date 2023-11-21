﻿using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Map;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using System.Linq;

namespace Toji.BetterMap.Handlers
{
    internal sealed class EventHandlers
    {
        public void OnGenerated()
        {
            BreakableDoor door = Door.Get(DoorType.NukeSurface).As<BreakableDoor>();

            door.IgnoredDamage |= Interactables.Interobjects.DoorUtils.DoorDamageType.Scp096;
            door.IgnoredDamage |= Interactables.Interobjects.DoorUtils.DoorDamageType.Grenade;

            door = Door.Get(DoorType.HID).As<BreakableDoor>();

            door.IgnoredDamage |= Interactables.Interobjects.DoorUtils.DoorDamageType.Grenade;
        }

        public void OnPlacingBulletHole(PlacingBulletHoleEventArgs ev)
        {
            ev.IsAllowed = false;
        }

        public void OnGeneratorActivated(GeneratorActivatingEventArgs ev)
        {
            System.Collections.Generic.IEnumerable<Player> computers = Player.Get(RoleTypeId.Scp079);

            if (!ev.IsAllowed || computers.Count() == 0)
            {
                return;
            }

            var generators = Generator.List.Count(x => x.IsEngaged) + 1;
            var xp = 50 * generators;

            foreach (Player ply in computers)
            {
                var scp = ply.Role.Base as Scp079Role;

                if (scp == null
                    || !scp.SubroutineModule.TryGetSubroutine(out Scp079TierManager tier)
                    || !scp.SubroutineModule.TryGetSubroutine(out Scp079LostSignalHandler lost))
                {
                    continue;
                }

                tier.ServerGrantExperience(xp, Scp079HudTranslation.ExpGainAdminCommand);

                lost.ServerLoseSignal(tier.AccessTierLevel switch
                {
                    5 => 10,
                    4 => 8,
                    3 => 6,
                    2 => 5,
                    _ => 3
                });
            }
        }
    }
}
