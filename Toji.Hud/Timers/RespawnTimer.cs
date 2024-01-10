using Exiled.API.Features;
using Mirror;
using PlayerRoles;
using PlayerRoles.Spectating;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Interfaces;
using Toji.ExiledAPI.Extensions;
using Toji.Global;
using Toji.Hud.API.Enums;
using Toji.Hud.API.Features;
using UnityEngine;
using Respawning;

namespace Toji.Hud.Timers
{
    public class RespawnTimer : Timer
    {
        private UserHint _secondHint;
        private UserHint _thirdHint;
        private UserHint _firstHint;
        private UserHint _lastHint;

        public override string Tag => "RespawnTimer";

        public override Func<Player, bool> Conditions => (Player player) => base.Conditions(player) && player.IsDead;

        protected override void Iteration(Player player, UserInterface component)
        {
            if (component == null || player == null)
            {
                return;
            }

            component.Add("RespawnTimer-First", _firstHint ??= BuildFirstHint());
            component.Add("RespawnTimer-Second", _secondHint ??= BuildSecondHint());
            component.Add("RespawnTimer-Third", _thirdHint ??= BuildThirdHint());
            component.Add("RespawnTimer-Last", _lastHint ??= BuildLastHint());
        }

        private UserHint BuildFirstHint()
        {
            var hint = new UserHint("<b><color=#ECF8F9>До следующего спавна: %time%\n%squad%</color></b>", 1, HintPosition.Top);

            hint.AddVariable("time", ParseTime);

            hint.AddVariable("squad", ParseTeam);

            return hint;
        }

        private UserHint BuildSecondHint()
        {
            var hint = new UserHint("<color=#068DA9><size=75%><align=left>Наблюдает: %died%</align>%adaptive_space%<align=right>Альфа-Боеголовка: %warhead%</align></size></color>", 1, HintPosition.Center);

            hint.AddAdaptiveSpace();

            hint.AddVariable("died", ParseSpectators);

            hint.AddVariable("warhead", ParseWarhead);

            return hint;
        }

        private UserHint BuildThirdHint()
        {
            var hint = new UserHint("<color=#068DA9><size=75%><align=left>С начала раунда прошло: %round%</align>%adaptive_space%<align=right>Генераторы: %generators%</align></size></color>", 1, HintPosition.Center);

            hint.AddAdaptiveSpace();

            hint.AddVariable("round", ParseRoundTime);

            hint.AddVariable("generators", ParseGenerators);

            return hint;
        }

        private UserHint BuildLastHint()
        {
            var hint = new UserHint("<b><color=%color%>Подкласс наблюдаемого: %class%</color></b>", 1, HintPosition.Bottom);

            hint.AddVariable("color", ParseColor);
            hint.AddVariable("class", ParseName);

            return hint;
        }

        private string ParseTime()
        {
            var time = TimeSpan.FromSeconds(Respawn.TimeUntilNextPhase + GetAdditionalTime());

            if (!Respawn.IsSpawning)
            {
                var team = Respawn.ChaosTickets > Respawn.NtfTickets ? SpawnableTeamType.ChaosInsurgency : SpawnableTeamType.NineTailedFox;

                time.Add(new TimeSpan(0, 0, team == SpawnableTeamType.NineTailedFox ? 18 : 13));
            }

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
            var team = GetSpawnTeam() switch
            {
                SpawnableTeamType.ChaosInsurgency => "группировка повстанцев хаоса",
                SpawnableTeamType.NineTailedFox => "отряд мобильно-оперативной группы",
                _ => "неизвестность",
            };

            return $"{(Respawn.IsSpawning ? "Прибывает:" : "Ожидается:")} {team}";
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

        private string ParseName(Player player)
        {
            if (!player.TryGetSpectatingPlayer(out var spectating))
            {
                return "отсутствует";
            }

            var subclass = spectating.GetSubclass();

            return (subclass == null) switch
            {
                true => "отсутствует",
                false => subclass.Name
            };
        }

        private string ParseColor(Player player)
        {
            if (!player.TryGetSpectatingPlayer(out var spectating))
            {
                return "#7E1717";
            }

            var subclass = spectating.GetSubclass();

            return (subclass == null) switch
            {
                true => "#7E1717",
                false => subclass is ICustomHintSubclass custom && !string.IsNullOrEmpty(custom.HintColor) ? custom.HintColor : "#7E1717"
            };
        }

        private int GetAdditionalTime()
        {
            var team = GetSpawnTeam();

            if (team == SpawnableTeamType.None || Respawn.IsSpawning)
            {
                return 0;
            }

            var value = 13;

            if (team == SpawnableTeamType.NineTailedFox)
            {
                value += 5;
            }

            return value;
        }

        private SpawnableTeamType GetSpawnTeam() => Respawn.IsSpawning switch
        {
            true => Respawn.NextKnownTeam,
            false => Respawn.ChaosTickets > Respawn.NtfTickets ? SpawnableTeamType.ChaosInsurgency : SpawnableTeamType.NineTailedFox,
        };
    }
}
