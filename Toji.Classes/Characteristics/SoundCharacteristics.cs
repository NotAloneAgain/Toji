using Exiled.API.Features;
using Toji.Classes.API.Features;

namespace Toji.Classes.Characteristics
{
    public class SoundCharacteristics : Characteristic<bool>
    {
        public SoundCharacteristics(bool value) : base(value) { }

        public override string Name => "Скрытность";

        protected override string GetAdvancedDescription(Player player) => GetDefaultDescription();

        protected override string GetDefaultDescription() => Value ? "Вы совершаете действия крайне тихо, SCP-939 не слышит ваших шагов и открытий дверей" : "Вы не будете скрытным (возможно, нерф).";
    }
}
