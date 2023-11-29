using Exiled.API.Features;
using Toji.Classes.API.Features;
using UnityEngine;

namespace Toji.Classes.Characteristics
{
    public class SizeCharacteristic : Characteristic<Vector3>
    {
        public SizeCharacteristic(Vector3 value) : base(value) { }

        public override string Name => "Измененный рост";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.Scale = Value;
        }

        public override void OnDisabled(Player player)
        {
            player.Scale = Vector3.one;

            base.OnDisabled(player);
        }

        public override string GetDesc(Player player = null) => Value.y * Value.x * Value.z > 1 ? "Ваш рост больше стандартного" : "Ваш рост меньше стандартного";
    }
}
