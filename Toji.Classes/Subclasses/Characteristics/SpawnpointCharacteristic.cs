using Exiled.API.Features;
using Toji.Classes.API.Features.Characteristics;
using Toji.Classes.API.Features.Spawnpoints;

namespace Toji.Classes.Subclasses.Characteristics
{
    public class SpawnpointCharacteristic : Characteristic<BaseSpawnpoint>
    {
        public SpawnpointCharacteristic(BaseSpawnpoint value) : base(value) { }

        public override string Name => "Свое место появления";

        public override string GetDesc(Player player = null) => $"Вы появляетесь в {Value.PointType}";

        public override void OnEnabled(Player player)
        {
            base.OnEnabled(player);

            player.Position = Value.Position;
        }
    }
}
