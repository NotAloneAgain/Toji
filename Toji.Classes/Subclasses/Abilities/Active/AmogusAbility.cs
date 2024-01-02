using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using PlayerRoles;
using System;
using Toji.Classes.API.Features.Abilities;
using Toji.ExiledAPI.Extensions;
using Toji.Global;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class AmogusAbility(uint cooldown, RoleTypeId target, RoleTypeId role, ItemType item = ItemType.None) : CooldownAbility(cooldown)
    {
        public override string Name => "Переодевание";

        public override string Desc => $"Ты можешь {(target.IsHuman() ? "переодеться" : "превратиться")} в {target.Translate()}, являясь {role.Translate()}";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if (player.Role.Type != role)
            {
                result = $"Ты должен быть {role.Translate()}";

                return false;
            }

            if (player.IsInPocketDimension)
            {
                result = "Да пошел ты нахуй";

                return false;
            }

            player.Role.Set(target, SpawnReason.Revived, RoleSpawnFlags.None);

            player.DisableAllEffects();

            if (!target.IsHuman())
            {
                player.DropAllWithoutKeycard();
            }

            if (item != ItemType.None)
            {
                if (player.IsInventoryFull)
                {
                    Pickup.CreateAndSpawn(item, player.Position, player.Rotation, player);
                }
                else
                {
                    player.AddItem(item);
                }
            }

            if (Cooldown == uint.MaxValue || Cooldown == 0)
            {
                IsEnabled = false;
            }

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
