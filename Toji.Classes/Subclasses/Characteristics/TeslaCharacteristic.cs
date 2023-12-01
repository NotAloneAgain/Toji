using Exiled.API.Features;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class TeslaCharacteristic : Characteristic<bool>
    {
        public TeslaCharacteristic(bool value) : base(value) { }

        public override string Name => "Особая реакция Тесла-Ворот";

        public override string GetDesc(Player player = null) => Value ? "Тесла-Ворота на вас не реагируют" : "Тесла-Ворота на вас реагируют";
    }
}
