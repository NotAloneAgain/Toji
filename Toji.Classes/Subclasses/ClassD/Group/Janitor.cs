using Exiled.API.Enums;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Features.Spawnpoints;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.ClassD.Group
{
    public class Janitor : DGroupSubclass, IHintSubclass, ILimitableGroup, IRandomSubclass
    {
        public override bool ShowInfo => true;

        public override string Name => "Уборщик";

        public override string Desc => "Убирался в комплексе, но узнав о нарушении У.С. SCP-Объектов решил эвакуироваться";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new SpawnpointCharacteristic(new RandomRoomSpawnpoint(RoomType.LczTCross, RoomType.LczStraight)),
            new InventoryCharacteristic(new List<Slot>(1)
            {
                new StaticSlot(ItemType.KeycardJanitor),
            }),
        };

        public int Chance => 25;

        public int Max => 4;
    }
}
