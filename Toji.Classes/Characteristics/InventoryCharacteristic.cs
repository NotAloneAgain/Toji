﻿using Exiled.API.Features;
using InventorySystem;
using PlayerRoles;
using System;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;

namespace Toji.Classes.Characteristics
{
    public class InventoryCharacteristic : Characteristic<List<Slot>>
    {
        private Dictionary<RoleTypeId, int> _defaultValues;

        public InventoryCharacteristic(List<Slot> value) : base(value) { }

        public override string Name => "Свой инвентарь";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            foreach (var slot in Value)
            {
                var item = slot.GetItem();

                if (item == ItemType.None)
                {
                    continue;
                }

                player.AddItem(item);
            }
        }

        protected override string GetAdvancedDescription(Player player) => GetDefaultDescription();

        protected override string GetDefaultDescription() => "Вы имеете свой уникальный инвентарь";
    }
}
