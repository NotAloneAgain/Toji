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
    public class KnockAbility : CooldownAbility
    {
        private float _distance;
        private HashSet<DoorType> _ignoredDoors;

        public KnockAbility(uint cooldown) : base(cooldown)
        {
            _distance = 5;
            _ignoredDoors = [

                DoorType.CheckpointArmoryA,
                DoorType.CheckpointArmoryB,
                DoorType.LczArmory,
                DoorType.HczArmory,
                DoorType.Scp049Armory,
                DoorType.HID,
                DoorType.Intercom
            ];
        }

        public KnockAbility(uint cooldown, float distance) : this(cooldown)
        {
            _distance = distance;
        }

        public KnockAbility(uint cooldown, float distance, HashSet<DoorType> ignoredDoors) : this(cooldown, distance)
        {
            _ignoredDoors = ignoredDoors;
        }

        public override string Name => "Выбивание дверей";

        public override string Desc => "Ты можешь выбивать некоторые двери с помощью своей силы или профессиональной подготовки";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.SendConsoleMessage($"Ты не сможешь выбить двери: {string.Join(", ", _ignoredDoors.Select(x => x.ToString()))}.", "red");
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
                result = "Ты не можешь выбивать двери, будучи связанным.";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (door == null)
            {
                result = $"Не удалось получить дверь (максимальная дистанция {_distance - 1}м).";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (!door.Is(out BreakableDoor breakable) || breakable.IsDestroyed || door.IsElevator || door.IsGate)
            {
                result = "Эту дверь нельзя выбить!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (door.IsLocked && door.DoorLockType is DoorLockType.Lockdown2176 or DoorLockType.Lockdown079)
            {
                result = $"Блокировка от системы безопасности, спровоцированной {door.DoorLockType switch
                {
                    DoorLockType.Lockdown2176 => "SCP-2176",
                    DoorLockType.Lockdown079 => "SCP-079",
                    _ => "НННЯ...ЕИЗВЕСТНО"
                }} не позволяет выбить дверь!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (_ignoredDoors.Contains(door.Type))
            {
                result = "Ты не можешь выбить дверь!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (!breakable.Damage(250, DoorDamageType.Grenade))
            {
                result = "Увы, у тебя не вышло!";

                AddUse(player, DateTime.Now, true, result);

                return false;
            }

            result = breakable.IsDestroyed switch
            {
                true => "Ты успешно выбил эту дверь!",
                _ => "Тебе не удалось выбить дверь, но она явна потерпела повреждения..."
            };

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
