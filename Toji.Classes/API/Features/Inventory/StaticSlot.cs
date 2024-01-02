namespace Toji.Classes.API.Features.Inventory
{
    public class StaticSlot(ItemType item) : Slot
    {
        public override ItemType GetItem() => item;
    }
}
