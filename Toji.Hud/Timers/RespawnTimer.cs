﻿using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Linq;
using Toji.Global;
using Toji.Hud.API.Enums;
using Toji.Hud.API.Features;
using UnityEngine;

namespace Toji.Hud.Timers
{
    public class RespawnTimer : Timer
    {
        private UserHint _afterCenterHint;
        private UserHint _topHint;

        public override string Tag => "RespawnTimer";

        public override Func<Player, bool> Conditions => (Player player) => base.Conditions(player) && player.IsDead;

        protected override void Iteration(Player player, UserInterface component)
        {
            component.Add("RespawnTimer-Top", BuildTopHint());
            component.Add("RespawnTimer-Center", BuildCenterHint());
        }

        private UserHint BuildTopHint()
        {
            if (_topHint == null)
            {
                var hint = new UserHint("<b><color=#068DA9>До следующего спавна: %time%\nОжидается: %squad%</color></b>", 1, HintPosition.Top);

                hint.AddVariable("time", ParseTime);

                hint.AddVariable("squad", ParseTeam);

                _topHint = hint;
            }

            return _topHint;
        }

        private UserHint BuildCenterHint()
        {
            if (_afterCenterHint == null)
            {
                var hint = new UserHint("<i><size=75%><align=left>Наблюдает: %died%</align><align=right>Альфа-Боеголовка: %warhead%</align>\n<align=left>С начала раунда прошло: %round%</align><align=right>Генераторы: %generators%</align></size></i>", 1, HintPosition.Bottom);

                hint.AddVariable("died", ParseSpectators);

                hint.AddVariable("round", ParseRoundTime);

                hint.AddVariable("warhead", ParseWarhead);

                hint.AddVariable("generators", ParseGenerators);

                _afterCenterHint = hint;
            }

            return _afterCenterHint;
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
