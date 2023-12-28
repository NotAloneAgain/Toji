using Exiled.API.Features;
using Toji.Classes.API.Features.Characteristics;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class ArtificalShieldCharacteristic : Characteristic<float>
    {
        private bool _persistent;
        private float _efficacy;
        private float _sustain;
        private float _limit;
        private float _decay;

        public ArtificalShieldCharacteristic(float value) : base(value)
        {
            _limit = 75;
            _decay = 1.2f;
            _efficacy = 0.7f;
            _sustain = 0;
            _persistent = false;
        }

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
