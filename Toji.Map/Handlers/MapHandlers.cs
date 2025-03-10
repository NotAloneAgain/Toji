﻿using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Map;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Pickups;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Toji.BetterMap.API;
using UnityEngine;

namespace Toji.BetterMap.Handlers
{
    internal sealed class MapHandlers
    {
        public void OnGenerated()
        {
            BreakableDoor door = Door.Get(DoorType.NukeSurface).As<BreakableDoor>();

            door.IgnoredDamage |= DoorDamageType.Scp096;
            door.IgnoredDamage |= DoorDamageType.Grenade;

            door = Door.Get(DoorType.HID).As<BreakableDoor>();

            door.IgnoredDamage |= DoorDamageType.Grenade;

            foreach (var generator in Generator.List)
            {
                NetworkServer.UnSpawn(generator.GameObject);

                generator.Transform.localScale *= 0.8f;

                NetworkServer.Spawn(generator.GameObject);
            }

            foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj == null)
                {
                    continue;
                }

                obj.TryOptimize();
            }
        }

        public void OnGeneratorActivated(GeneratorActivatingEventArgs ev)
        {
            IEnumerable<Player> computers = Player.List.Where(ply => ply.Role.Type == RoleTypeId.Scp079);

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
