using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class RepairDoorAbility : CooldownAbility
    {
        private float _distance;
        private HashSet<DoorType> _ignoredDoors;

        public RepairDoorAbility(uint cooldown) : base(cooldown)
        {
            _distance = 5;
            _ignoredDoors = new HashSet<DoorType>()
            {
                DoorType.CheckpointArmoryA,
                DoorType.CheckpointArmoryB,
                DoorType.LczArmory,
                DoorType.HczArmory,
                DoorType.Scp049Armory,
                DoorType.HID,
                DoorType.Intercom
            };
        }

        public RepairDoorAbility(uint cooldown, float distance) : this(cooldown)
        {
            _distance = distance;
        }

        public RepairDoorAbility(uint cooldown, float distance, HashSet<DoorType> ignoredDoors) : this(cooldown, distance)
        {
            _ignoredDoors = ignoredDoors;
        }

        public override string Name => "Починка дверей";

        public override string Desc => "Ты можешь чинить некоторые выбитые двери с помощью своих знаний";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.SendConsoleMessage($"Ты не сможешь чинить: {string.Join(", ", _ignoredDoors.Select(x => x.ToString()))}.", "red");
        }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var door = Door.GetClosest(player.Position, out var distance);

            if (player.IsCuffed)
            {
                result = "Ты не можешь чинить двери, будучи связанным.";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (door == null || distance > _distance)
            {
                result = $"Не удалось получить дверь (максимальная дистанция {_distance - 1}м).";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (!door.Is(out BreakableDoor breakable) || !breakable.IsDestroyed || door.IsElevator || door.IsGate || _ignoredDoors.Contains(door.Type))
            {
                result = "Эту дверь нельзя починить!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (_ignoredDoors.Contains(door.Type))
            {
                result = "Ты не можешь починить эту дверь!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            breakable.IsDestroyed = false;
            breakable.Base._prevDestroyed = false;
            breakable.Base._nonInteractable = false;

            if (breakable.MaxHealth <= 0)
            {
                breakable.MaxHealth = 100;
            }

            breakable.Health = breakable.MaxHealth;

            breakable.GameObject.SetActive(true);

            if (breakable.Base._objectToReplace)
            {
                breakable.Base._objectToReplace.SetActive(true);

                var airlock = AirlockController.Get(breakable);

                if (airlock != null)
                {
                    airlock.AirlockDisabled = false;
                }
            }

            NetworkServer.Spawn(breakable.GameObject);

            foreach (var ply in Player.List)
            {
                NetworkServer.SendSpawnMessage(breakable.Base.netIdentity, ply.Connection);
            }

            result = "Ты успешно починил эту дверь!";

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
