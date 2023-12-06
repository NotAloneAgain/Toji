using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
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

        public override string Desc => "Неважно кого убить, тебе нужно больше смертей и крови";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
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

        public int Chance => 18;

        public int Max => 3;

        public void Subscribe() => Exiled.Events.Handlers.Map.SpawningItem += OnSpawningItem;

        public void Unsubscribe() => Exiled.Events.Handlers.Map.SpawningItem -= OnSpawningItem;

        public void OnSpawningItem(SpawningItemEventArgs ev)
        {
            if (ev.Pickup.Type != ItemType.GunCOM15)
            {
                return;
            }

            _room = ev.Pickup.Room.Type;
        }

        protected override void OnAdded(in Player player, bool withAbility)
        {
            if (player.HasItem(ItemType.GunCOM15))
            {
                player.SendConsoleMessage("Ваш пистолет находится в ваших руках.", "green");

                return;
            }

            player.SendConsoleMessage(Translate(_room), "yellow");
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
