using Exiled.API.Features;
using System;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class AttackCooldownCharacteristic : Characteristic<float>
    {
        public AttackCooldownCharacteristic(float value) : base(value) { }

        public override string Name => "Измененная перезарядка";

        public override string GetDesc(Player player = null) => $"Длительность перезарядки вашей атаки нестандартна, составляет {Math.Round(Value, 2)} секунды.";
    }
}
