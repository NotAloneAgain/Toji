using Exiled.API.Enums;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using System.Collections.Generic;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.Subclasses.ClassD.Group
{
    public class Killer : DGroupSubclass, ICustomDamageSubclass, IHasInventory, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        private RoomType _room;

        public override string Name => "Убийца";

        public override string Desc => "Всегда готов кого-либо убить и знаешь много методов для этого";

        public int Chance => 90;

        public int Max => 3;

        public float Damage => -2;

        public float DamageMultiplayer => 1.1f;

        public HashSet<Slot> Slots { get; } = new HashSet<Slot>()
        {
            new RandomSlot(new Dictionary<ItemType, int>(2)
            {
                { ItemType.GunCOM15, 50 },
                { ItemType.Medkit, 100 }
            }),
            new RandomSlot(new Dictionary<ItemType, int>(2)
            {
                { ItemType.Adrenaline, 50 },
                { ItemType.Painkillers, 100 }
            }),
            new StaticSlot(ItemType.Painkillers),
        };

        public override bool Assign(in Exiled.API.Features.Player player)
        {
            if (!base.Assign(player))
            {
                return false;
            }

            player.SendConsoleMessage(Translate(_room), "yellow");

            return true;
        }

        public override void Subscribe()
        {
            base.Subscribe();

            Map.SpawningItem += OnSpawningItem;
        }

        public override void Unsubscribe()
        {
            Map.SpawningItem -= OnSpawningItem;

            base.Unsubscribe();
        }

        public void OnDamage(HurtingEventArgs ev) { }

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
                RoomType.LczCafe => "Младший сотрудник СБ забыл свой пистолет в PC-00",
                RoomType.Lcz173 => "Крыса внутри СБ оставила пистолет в PT-00",
                RoomType.LczToilets => "Твои сообщники успешно спрятали пистолет в WC-00",
                RoomType.LczGlassBox => "Один из инженеров забыл свой пистолет в GR-00",
                RoomType.Lcz330 => "Один из инженеров забыл свой пистолет в TC-00",
                _ => "неизвестно..."
            };
        }
    }
}
