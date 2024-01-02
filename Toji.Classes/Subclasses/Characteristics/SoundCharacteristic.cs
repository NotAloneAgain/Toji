using Exiled.API.Features;
using Toji.Classes.API.Features.Characteristics;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class SoundCharacteristic(bool value) : Characteristic<bool>(value)
    {
        public override string Name => "Скрытность";

        public override string GetDesc(Player player = null) => Value ? "Вы совершаете действия крайне тихо, SCP-939 не слышит ваших шагов и открытий дверей" : "Вы не будете скрытным (возможно, нерф).";
    }
}
