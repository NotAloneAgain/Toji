using Exiled.API.Features;
using Toji.Classes.API.Features.Characteristics;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class ArtificalShieldCharacteristic(float value) : Characteristic<float>(value)
    {
        private bool _persistent = false;
        private float _efficacy = 0.7f;
        private float _sustain = 0;
        private float _limit = 75;
        private float _decay = 1.2f;

        public ArtificalShieldCharacteristic(float value, float limit, float decay, float efficacy, float sustain, bool persistent) : this(value)
        {
            _limit = limit;
            _decay = decay;
            _efficacy = efficacy;
            _sustain = sustain;
            _persistent = persistent;
        }

        public override string Name => "Щит";

        public override string GetDesc(Player player = null) => "Ты имеешь щит, готовый взять на себя часть твоего урона";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.AddAhp(Value, _limit, _decay, _efficacy, _sustain, _persistent);
        }

        public override void OnDisabled(Player player)
        {
            player.AddAhp(0, 75);
            player.ArtificialHealth = 0;

            base.OnDisabled(player);
        }
    }
}
