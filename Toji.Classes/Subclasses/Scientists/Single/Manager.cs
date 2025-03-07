﻿using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using Respawning;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Features.Spawnpoints;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Passive;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scientists.Single
{
    public class Manager : ScientistSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Менеджер Комплекса";

        public override string Desc => "Элитный управленец, годами управлявший комплексом";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            TeslaReactionAbility.IsIgnored,
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new SpawnpointCharacteristic(new DoorSpawnpoint(DoorType.NukeArmory)),
            new InventoryCharacteristic(new List<Slot>(4)
            {
                new StaticSlot(ItemType.KeycardFacilityManager),
                new StaticSlot(ItemType.Medkit),
                new StaticSlot(ItemType.Radio),
                new StaticSlot(ItemType.ArmorLight),
            })
        };

        public int Chance => 10;

        protected internal override void OnEscaped(in Player player)
        {
            base.OnEscaped(player);

            if (player.Role.Type == RoleTypeId.NtfSpecialist)
            {
                Respawn.GrantTickets(SpawnableTeamType.NineTailedFox, 2);
            }
            else
            {
                Respawn.GrantTickets(SpawnableTeamType.ChaosInsurgency, 1);
            }
        }
    }
}
