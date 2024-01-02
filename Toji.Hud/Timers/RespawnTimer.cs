using Exiled.API.Features;
using System;
using Toji.Global;
using Toji.Hud.API.API;

namespace Toji.Hud.Timers
{
    public class RespawnTimer : Timer
    {
        public override string Tag => "RespawnTimer";

        public override Func<Player, bool> Conditions => (Player player) => base.Conditions(player) && player.IsDead;

        protected override UserHint BuildHint(Player player)
        {
            var hint = new UserHint("До следующего спавна: %time%\nОжидается: %squad%", 0.5f);

            hint.AddVariable("time", delegate ()
            {
                var time = Respawn.TimeUntilSpawnWave;

                return $"{time.Minutes.GetMinutesString()} {time.Seconds.GetSecondsString()}";
            });

            hint.AddVariable("squad", delegate ()
            {
                var team = Respawn.IsSpawning switch
                {
                    true => Respawn.NextKnownTeam,
                    false => Respawn.ChaosTickets > Respawn.NtfTickets ? Respawning.SpawnableTeamType.ChaosInsurgency : Respawning.SpawnableTeamType.NineTailedFox,
                };

                return team switch
                {
                    Respawning.SpawnableTeamType.ChaosInsurgency => "группировка повстанцев хаоса",
                    Respawning.SpawnableTeamType.NineTailedFox => "отряд мобильно-оперативной группы",
                    _ => "неизвестность",
                };
            });

            return hint;
        }
    }
}
