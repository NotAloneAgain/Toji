using Exiled.API.Features;
using Toji.Classes.API.Features;
using UnityEngine;

namespace Toji.Classes.Characteristics
{
    public class HurtMultiplayerCharacteristic : Characteristic<float>
    {
        public HurtMultiplayerCharacteristic(float value) : base(value) { }

        public override string Name => "Модификатор урона";

        public override string GetDesc(Player player = null)
        {
            var value = Value - 1;

            return value > 0 ? $"Входящий урон повышен на {value * 100}%" : $"Входящий урон понижен на {Mathf.Abs(value * 100)}%";
        }
    }
}
