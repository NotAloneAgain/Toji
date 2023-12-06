using Exiled.API.Enums;
using Exiled.API.Features;
using Toji.Classes.API.Features;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class EffectsCharacteristic : Characteristic<EffectType[]>
    {
        public EffectsCharacteristic(params EffectType[] value) : base(value) { }

        public override string Name => "Эффекты";

        public override string GetDesc(Player player = null) => "При появлении ты имеешь некоторые эффекты";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            foreach (var effect in Value)
            {
                player.EnableEffect(effect);
            }
        }

        public override void OnDisabled(Player player)
        {
            foreach (var effect in Value)
            {
                player.DisableEffect(effect);
            }

            base.OnDisabled(player);
        }
    }
}
