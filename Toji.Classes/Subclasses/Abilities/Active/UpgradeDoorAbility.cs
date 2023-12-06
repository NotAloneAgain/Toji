using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class UpgradeDoorAbility : CooldownAbility
    {
        private float _distance;
        private HashSet<DoorType> _ignoredDoors;

        public UpgradeDoorAbility(uint cooldown) : base(cooldown)
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

        public UpgradeDoorAbility(uint cooldown, float distance) : this(cooldown)
        {
            _distance = distance;
        }

        public UpgradeDoorAbility(uint cooldown, float distance, HashSet<DoorType> ignoredDoors) : this(cooldown, distance)
        {
            _ignoredDoors = ignoredDoors;
        }

        public override string Name => "Улучшение дверей";

        public override string Desc => "Ты можешь улучшать некоторые двери с помощью своих знаний";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.SendConsoleMessage($"Ты не сможешь улучшить двери: {string.Join(", ", _ignoredDoors.Select(x => x.ToString()))}.", "red");
        }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            var door = player.GetDoorFromView(_distance);

            if (player.IsCuffed)
            {
                result = "Ты не можешь улучшать двери, будучи связанным.";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (door == null)
            {
                result = $"Не удалось получить дверь (максимальная дистанция {_distance - 1}м).";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (!door.Is(out BreakableDoor breakable) || breakable.IsDestroyed || door.IsElevator || door.IsGate || _ignoredDoors.Contains(door.Type))
            {
                result = "Эту дверь нельзя улучшить!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (_ignoredDoors.Contains(door.Type))
            {
                result = "Ты не можешь улучшить эту дверь!";

                AddUse(player, System.DateTime.Now, false, result);

                return false;
            }

            breakable.IgnoredDamage |= DoorDamageType.Grenade;
            breakable.IgnoredDamage |= DoorDamageType.Scp096;

            result = "Ты успешно улучшил эту дверь!";

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
