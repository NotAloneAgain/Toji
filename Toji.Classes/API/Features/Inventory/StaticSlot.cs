namespace Toji.Classes.API.Features.Inventory
{
    public class StaticSlot : Slot
    {
        private readonly ItemType _item;

        public StaticSlot(ItemType item) => _item = item;

        public override ItemType GetItem() => _item;
    }
}
