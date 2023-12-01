using Exiled.API.Enums;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.Handlers;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Group
{
    public class Killer : DGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass, ISubscribable
    {
        private RoomType _room;

        public override string Name => "Убийца";

        public override string Desc => "Всегда готов отнять жизнь и знаешь лучшие методики";

        public int Chance => 18;

        public int Max => 3;

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>()
        {
            new DamageMultiplayerCharacteristic(1.1f),
            new InventoryCharacteristic(new List<Slot>()
            {
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.GunCOM15, 44 },
                    { ItemType.Medkit, 100 }
                }),
                new RandomSlot(new Dictionary<ItemType, int>(2)
                {
                    { ItemType.Adrenaline, 48 },
                    { ItemType.Painkillers, 100 }
                }),
            })
        };

        public override bool Assign(in Exiled.API.Features.Player player)
        {
            if (!base.Assign(player))
            {
                return false;
            }

            if (player.HasItem(ItemType.GunCOM15))
            {
                player.SendConsoleMessage("Ваш пистолет находится в ваших руках.", "green");

                return true;
            }

            player.SendConsoleMessage(Translate(_room), "yellow");

            return true;
        }

        public void Subscribe() => Map.SpawningItem += OnSpawningItem;

        public void Unsubscribe() => Map.SpawningItem -= OnSpawningItem;

        public void OnSpawningItem(SpawningItemEventArgs ev)
        {
            if (ev.Pickup.Type != ItemType.GunCOM15)
            {
                return;
            }

            _room = ev.Pickup.Room.Type;
        }

        private string Translate(RoomType type)
        {
            return type switch
            {
                RoomType.LczCafe => "Младший сотрудник СБ забыл свой пистолет в PC-15.",
                RoomType.Lcz173 => "Крыса внутри СБ оставила пистолет в PT-00.",
                RoomType.LczToilets => "Твои сообщники успешно спрятали пистолет в WC-00.",
                RoomType.LczGlassBox => "Один из инженеров забыл свой пистолет в GR-18.",
                RoomType.Lcz330 => "Один из инженеров забыл свой пистолет в TC-01.",
                _ => "неизвестно..."
            };
        }
    }
}
