using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Linq;
using Toji.Global;
using Toji.Hud.API.API;
using Toji.Hud.API.Enums;
using UnityEngine;

namespace Toji.Hud.Timers
{
    public class RespawnTimer : Timer
    {
        public override string Tag => "RespawnTimer";

        public override Func<Player, bool> Conditions => (Player player) => base.Conditions(player) && player.IsDead;

        protected override void Iteration(Player player, UserInterface component)
        {
            component.Add(Tag, BuildTopHint());
            component.Add(Tag, BuildCenterHint());
        }

        private UserHint BuildTopHint()
        {
            var hint = new UserHint("<b><color=#068DA9>До следующего спавна: %time%\nОжидается: %squad%</color></b>", 0.5f, HintPosition.Top);

            hint.AddVariable("time", ParseTime);

            hint.AddVariable("squad", ParseTeam);

            return hint;
        }

        private UserHint BuildCenterHint()
        {
            var hint = new UserHint("\n\n\n<align=left>Наблюдает: %died%\nС начала раунда прошло: %round%</align>\n<align=right>Альфа-Боеголовка: %warhead%\nГенераторы: %generators%</align>", 0.5f, HintPosition.Center);

            hint.AddVariable("died", ParseSpectators);

            hint.AddVariable("round", ParseRoundTime);

            hint.AddVariable("warhead", ParseWarhead);

            hint.AddVariable("generators", ParseGenerators);

            return hint;
        }

        private string ParseTime()
        {
            var time = Respawn.TimeUntilSpawnWave;

            string result = string.Empty;

            if (time.Minutes > 0)
            {
                result += time.Minutes.GetMinutesString();

                result += " ";
            }

            if (time.Seconds > 0)
            {
                result += time.Seconds.GetSecondsString();
            }

            if (string.IsNullOrEmpty(result))
            {
                result = "чуть-чуть";
            }

            return result;
        }

        private string ParseTeam()
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
        }

        private string ParseSpectators()
        {
            return Player.List.Where(ply => ply.Role.Type == RoleTypeId.Spectator).Count().GetPlayersString();
        }

        private string ParseRoundTime()
        {
            var time = Round.ElapsedTime;

            string result = string.Empty;

            if (time.Minutes > 0)
            {
                result += time.Minutes.GetMinutesString();

                result += " ";
            }

            if (time.Seconds > 0)
            {
                result += time.Seconds.GetSecondsString();
            }

            if (string.IsNullOrEmpty(result))
            {
                result = "чуть-чуть";
            }

            return result;
        }

        private string ParseWarhead()
        {
            if (Warhead.IsDetonated)
            {
                return "Взорвана";
            }

            if (Warhead.IsInProgress)
            {
                var minutes = Mathf.RoundToInt(Warhead.DetonationTimer / 60);
                var seconds = Mathf.RoundToInt(Warhead.DetonationTimer % 60);

                string result = "осталось ";

                if (minutes > 0)
                {
                    result += minutes.GetMinutesString();

                    result += " ";
                }

                if (seconds > 0)
                {
                    result += seconds.GetSecondsString();
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = "чуть-чуть";
                }

                return result;
            }

            return Warhead.LeverStatus switch
            {
                true => "включена",
                false => "выключена"
            };
        }

        private string ParseGenerators()
        {
            return $"{Generator.List.Count(gen => gen.IsEngaged)}/3"; ;
        }
    }
}
