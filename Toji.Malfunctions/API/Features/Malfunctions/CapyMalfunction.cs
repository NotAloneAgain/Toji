using Exiled.API.Extensions;
using Exiled.API.Features;
using System.Linq;

namespace Toji.Malfunctions.API.Features.Malfunctions
{
    public class CapyMalfunction : BaseMalfunction
    {
        public override string Name => "Дело капибары";

        public override int MinDuration => 0;

        public override int MaxDuration => 0;

        public override int Cooldown => 190;

        public override int Chance => 43;

        public override int Delay => 80;

        public override void Activate(int duration)
        {
            base.Activate(duration);

            var players = Player.List.Where(x => x.IsHuman);

            if (!players.Any())
            {
                return;
            }

            Map.Broadcast(12, $"<color=#780000><b>В комплексе обнаружено КАПИБАРА! Заведено {Name.ToLower()} для расследования</b></color>");

            Scp956.SpawnBehindTarget(players.GetRandomValue());
        }
    }
}
