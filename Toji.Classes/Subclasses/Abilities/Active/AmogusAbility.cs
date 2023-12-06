using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using PlayerRoles;
using System;
using Toji.Classes.API.Features.Abilities;
using Toji.Global;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class AmogusAbility : CooldownAbility
    {
        private RoleTypeId _target;
        private RoleTypeId _role;
        private ItemType _item;

        public AmogusAbility(uint cooldown, RoleTypeId target, RoleTypeId role, ItemType item = ItemType.None) : base(cooldown)
        {
            _target = target;
            _role = role;
            _item = item;
        }

        public override string Name => "Переодевание";

        public override string Desc => $"Ты можешь переодеться в {_target.Translate()}, являясь {_role.Translate()}";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (player.Role.Type != _role)
            {
                result = $"Ты должен быть {_role.Translate()}";

                return false;
            }

            player.Role.Set(_target, SpawnReason.Revived, RoleSpawnFlags.None);

            player.DisableAllEffects();

            if (_item != ItemType.None)
            {
                if (player.IsInventoryFull)
                {
                    Pickup.CreateAndSpawn(_item, player.Position, player.Rotation, player);
                }
                else
                {
                    player.AddItem(_item);
                }
            }

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
