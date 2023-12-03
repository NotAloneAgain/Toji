using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Features;
using Toji.Classes.Subclasses.Characteristics;
using Toji.Classes.API.Interfaces;
using Toji.Classes.API.Enums;

namespace Toji.Classes.Subclasses.Guards.Group
{
    public class Assault : GuardGroupSubclass, IHintSubclass, IPrioritySubclass
    {
        public override string Name => "Штурмовик";

        public override string Desc => "";

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(3)
        {
            new HealthCharacteristic(125),
            new InventoryCharacteristic(new List<Slot>(2)
            {
                new StaticSlot(ItemType.Adrenaline),
                new StaticSlot(ItemType.Painkillers),
            })
        };

        public LoadPriority Priority => LoadPriority.Lowest;
    }
}
