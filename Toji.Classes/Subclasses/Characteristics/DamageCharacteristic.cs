using Exiled.API.Features;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class DamageCharacteristic : Characteristic<float>
    {
        public DamageCharacteristic(float value) : base(value) { }

        public override string Name => "Измененный урон";

        public override string GetDesc(Player player = null) => $"{Value} урона от атаки.";
    }
}
