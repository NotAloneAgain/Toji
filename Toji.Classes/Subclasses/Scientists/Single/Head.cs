﻿using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using Respawning;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Features.Spawnpoints;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scientists.Single
{
    public class Head : ScientistSingleSubclass, IHintSubclass, IRandomSubclass
    {
        public override string Name => "Научный руководитель";

        public override string Desc => "Отличный лидер и ученый, главенствующий над всем научным отделом";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new SpawnpointCharacteristic(new RandomDoorSpawnpoint(DoorType.Scp330, DoorType.LczCafe)),
            new InventoryCharacteristic(new List<Slot>(4)
            {
                new StaticSlot(ItemType.KeycardResearchCoordinator),
                new StaticSlot(ItemType.Medkit),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.Medkit, 50 },
                    { ItemType.Painkillers, 100 }
                }),
                new StaticSlot(ItemType.Radio),
            })
        };

        public int Chance => 12;

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
