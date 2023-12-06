using Exiled.API.Enums;
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
        public override bool ShowInfo => true;

        public override string Name => "Менеджер Комплекса";

        public override string Desc => "Элитный управленец, годами управлявший комплексом";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            TeslaReactionAbility.IsIgnored,
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new SpawnpointCharacteristic(new DoorSpawnpoint(DoorType.NukeArmory)),
            new InventoryCharacteristic(new List<Slot>(8)
            {
                new StaticSlot(ItemType.KeycardGuard),
                new StaticSlot(ItemType.GunFSP9),
                new StaticSlot(ItemType.Medkit),
                new StaticSlot(ItemType.Painkillers),
                new StaticSlot(ItemType.Radio),
            })
        };

        public int Chance => 8;

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
