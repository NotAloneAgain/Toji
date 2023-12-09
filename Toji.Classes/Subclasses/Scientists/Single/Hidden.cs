using PlayerRoles;
using System.Collections.Generic;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Inventory;
using Toji.Classes.API.Interfaces;
using Toji.Classes.Subclasses.Abilities.Active;
using Toji.Classes.Subclasses.Characteristics;

namespace Toji.Classes.Subclasses.Scientists.Single
{
    public class Hidden : ScientistSingleSubclass, IHintSubclass, IRandomSubclass, INeedRole
    {
        public override string Name => "Скрытный";

        public override string Desc => "Очень незаметная и скрытная персона, которая не любит лишнее внимание";

        public override List<BaseAbility> Abilities { get; } = new List<BaseAbility>(1)
        {
            new HideAbility(100, 15),
        };

        public override List<BaseCharacteristic> Characteristics { get; } = new List<BaseCharacteristic>(2)
        {
            new InventoryCharacteristic(new List<Slot>(2)
            {
                new StaticSlot(ItemType.KeycardScientist),
                new StaticSlot(ItemType.Painkillers),
            })
        };

        public RoleTypeId NeedRole => RoleTypeId.Scp939;

        public int Chance => 15;
    }
}
