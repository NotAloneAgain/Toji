using System.Collections.Generic;
using UnityEngine;

namespace Toji.Classes.API.Features.Inventory
{
    public abstract class Slot
    {
        public abstract ItemType GetItem();
    }
}
