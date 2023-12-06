using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using Respawning;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Features.Spawnpoints;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scientists.Single
{
    public class Engineer : ScientistSingleSubclass, IHintSubclass, IRandomSubclass, ISpawnpointSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "Инженер";

        public override string Desc => "Ремонтировал старую камеру содержания SCP-173, но узнав о нарушении У.С. SCP-Объектов решил эвакуироваться";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(2)
        {
            new UpgradeDoorAbility(120, 5),
            new RepairDoorAbility(100, 5),
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(1)
        {
            new InventoryCharacteristic(new List<Slot>(8)
            {
                new StaticSlot(ItemType.KeycardContainmentEngineer),
                new StaticSlot(ItemType.Medkit),
                new StaticSlot(ItemType.Radio),
                new StaticSlot(ItemType.Flashlight),
            })
        };

        public BaseSpawnpoint Spawnpoint { get; } = new DoorSpawnpoint(DoorType.Scp173Connector);

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
