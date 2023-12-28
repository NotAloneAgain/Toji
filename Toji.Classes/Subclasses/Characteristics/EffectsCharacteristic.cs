using Exiled.API.Enums;
using Exiled.API.Features;
using Toji.Classes.API.Features.Characteristics;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class EffectsCharacteristic : Characteristic<EffectType[]>
    {
        private byte _intensity;
        private float _duration;

        public EffectsCharacteristic(params EffectType[] value) : base(value)
        {
            _intensity = 1;
            _duration = 0;
        }

        public EffectsCharacteristic(byte intensity, float duration, params EffectType[] value) : this(value)
        {
            _intensity = intensity;
            _duration = duration;
        }

        public override string Name => "Эффекты";

        public override string GetDesc(Player player = null) => "При появлении ты имеешь некоторые эффекты";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            foreach (var effect in Value)
            {
                player.EnableEffect(effect, _intensity, _duration);
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
