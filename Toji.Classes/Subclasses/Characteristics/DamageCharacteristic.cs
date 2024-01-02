using Exiled.API.Features;
using Toji.Classes.API.Features.Characteristics;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class DamageCharacteristic(float value) : Characteristic<float>(value)
    {
        public override string Name => "Измененный урон";

        public override string GetDesc(Player player = null) => $"{Value} урона от атаки.";
    }
}
