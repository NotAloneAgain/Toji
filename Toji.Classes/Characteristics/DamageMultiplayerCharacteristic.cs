using Exiled.API.Features;
using Toji.Classes.API.Features;
using UnityEngine;

namespace Toji.Classes.Characteristics
{
    public class DamageMultiplayerCharacteristic : Characteristic<float>
    {
        public DamageMultiplayerCharacteristic(float value) : base(value) { }

        public override string Name => "Модификатор урона";

        protected override string GetAdvancedDescription(Player player) => GetDefaultDescription();

        protected override string GetDefaultDescription()
        {
            var value = Value - 1;

            return value > 0 ? $"Исходящий урон повышен на {value * 100}%" : $"Исходящий урон понижен на {Mathf.Abs(value * 100)}%";
        }
    }
}
