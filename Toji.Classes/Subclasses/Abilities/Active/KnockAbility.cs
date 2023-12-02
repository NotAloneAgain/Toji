using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            player.SendConsoleMessage($"Ты не сможешь выбивать: {string.Join(", ", _ignoredDoors.Select(x => x.ToString()))}", "red");
        }

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                return false;
            }

            var door = player.GetDoorFromView(_distance);

            if (player.IsCuffed)
            {
                return false;
            }

            if (door == null)
            {
                return false;
            }

            if (!door.Is(out BreakableDoor breakable) || breakable.IsDestroyed || door.IsElevator || door.IsLocked && door.DoorLockType is DoorLockType.Lockdown2176 or DoorLockType.Lockdown079 || door.IsGate || _ignoredDoors.Contains(door.Type))
            {
                return false;
            }

            if (!breakable.Damage(250, DoorDamageType.Grenade))
            {
                return false;
            }

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
