using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using PlayerRoles.Spectating;
using System;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Interfaces;
using Toji.ExiledAPI.Extensions;
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
            if (component == null || player == null)
            {
                return;
            }

            component.Add("RespawnTimer-Top", BuildTopHint());
            component.Add("RespawnTimer-Center", BuildCenterHint());

            if (player.TryGetSpectatingPlayer(out var spectating))
            {
                component.Add("RespawnTimer-Bottom", BuildBottomHint(spectating));
            }
        }

        private UserHint BuildTopHint()
        {
            if (_topHint == null)
            {
                var hint = new UserHint("<b><color=#ECF8F9>До следующего спавна: %time%\nОжидается: %squad%</color></b>", 1, HintPosition.Top);

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
                var hint = new UserHint("<color=#068DA9><size=75%><align=left>Наблюдает: %died%</align><space=18em><align=right>Альфа-Боеголовка: %warhead%</align>\n<align=left>С начала раунда прошло: %round%</align><space=18em><align=right>Генераторы: %generators%</align></size></color>", 1, HintPosition.Bottom);

                hint.AddVariable("died", ParseSpectators);

                hint.AddVariable("round", ParseRoundTime);

                hint.AddVariable("warhead", ParseWarhead);

                hint.AddVariable("generators", ParseGenerators);

                _afterCenterHint = hint;
            }

            return _afterCenterHint;
        }

        private UserHint BuildBottomHint(Player player)
        {
            var subclass = player.GetSubclass();

            (string role, string color) = (subclass == null) switch
            {
                true => ("отсутствует", "#7E1717"),
                false => (subclass.Name, subclass is ICustomHintSubclass custom && !string.IsNullOrEmpty(custom.HintColor) ? custom.HintColor : "#7E1717")
            };

            var hint = new UserHint($"<b><color={color}>Подкласс наблюдаемого: {role}</color></b>", 1, HintPosition.Bottom);

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
                var seconds = Mathf.RoundToInt(Warhead.DetonationTimer);

                return $"осталось {seconds.GetSecondsString()}";
            }

            return Warhead.LeverStatus switch
            {
                true => Warhead.Controller.NetworkCooldownEndTime <= NetworkTime.time ? "включена" : "перезапуск",
                false => "выключена"
            };
        }

        private string ParseGenerators()
        {
            return $"{Generator.List.Count(gen => gen.IsEngaged)}/3"; ;
        }
    }
}
