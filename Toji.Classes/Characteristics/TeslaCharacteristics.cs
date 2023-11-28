using Exiled.API.Features;
using Toji.Classes.API.Features;

namespace Toji.Classes.Characteristics
{
    public class TeslaCharacteristics : Characteristic<bool>
    {
        public TeslaCharacteristics(bool value) : base(value) { }

        public override string Name => "Особая реакция Тесла-Ворот";

        protected override string GetAdvancedDescription(Player player) => GetDefaultDescription();

        protected override string GetDefaultDescription() => Value ? "Тесла-Ворота на вас не реагируют" : "Тесла-Ворота на вас реагируют";
    }
}
