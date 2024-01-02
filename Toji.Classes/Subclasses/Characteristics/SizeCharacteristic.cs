using Exiled.API.Features;
using Toji.Classes.API.Features.Characteristics;
using UnityEngine;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class SizeCharacteristic(Vector3 value) : Characteristic<Vector3>(value)
    {
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
