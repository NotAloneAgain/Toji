using System.Collections.Generic;
using Toji.Classes.API.Features.Inventory;

namespace Toji.Classes.API.Interfaces
{
    public interface IHasInventory
    {
        public HashSet<Slot> Slots { get; }
    }
}
