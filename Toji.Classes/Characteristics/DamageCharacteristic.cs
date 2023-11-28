using Exiled.API.Features;
using Toji.Classes.API.Features;

namespace Toji.Classes.Characteristics
{
    public class DamageCharacteristic : Characteristic<float>
    {
        public DamageCharacteristic(float value) : base(value) { }

        public override string Name => "Измененный урон";

        protected override string GetAdvancedDescription(Player player) => GetDefaultDescription();

        protected override string GetDefaultDescription() => $"{Value} урона от атаки.";
    }
}
