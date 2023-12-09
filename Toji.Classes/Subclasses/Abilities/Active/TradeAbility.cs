using Exiled.API.Extensions;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using Toji.Classes.API.Features.Abilities;

namespace Toji.Classes.Subclasses.Abilities.Active
{
    public class TradeAbility : CooldownAbility
    {
        private static readonly HashSet<ItemType> _blocked;

        static TradeAbility() => _blocked = new HashSet<ItemType>()
            {
                ItemType.None,
                ItemType.Ammo12gauge,
                ItemType.Ammo44cal,
                ItemType.Ammo556x45,
                ItemType.Ammo762x39,
                ItemType.Ammo9x19,
                ItemType.ArmorCombat,
                ItemType.ArmorHeavy,
                ItemType.ArmorLight
            };

        public TradeAbility(uint cooldown) : base(cooldown) { }

        public override string Name => "Обмен";

        public override string Desc => "Ты можешь обменять монетку на случайный предмет";

        public override bool Activate(Player player, out object result)
        {
            if (!base.Activate(player, out result))
            {
                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            if ((player.CurrentItem?.Type ?? ItemType.None) == ItemType.Coin)
            {
                result = "Ты не держишь монетку в руках!";

                AddUse(player, DateTime.Now, false, result);

                return false;
            }

            ItemType item = ItemType.None;

            do
            {
                item = Enum.GetValues(typeof(ItemType)).ToArray<ItemType>().GetRandomValue();
            } while (_blocked.Contains(item));

            player.RemoveHeldItem();

            player.AddItem(item);

            result = "Успешный обмен!";

            AddUse(player, DateTime.Now, true, result);

            return true;
        }
    }
}
