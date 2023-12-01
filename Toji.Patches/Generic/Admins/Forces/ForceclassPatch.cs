﻿using CommandSystem;
using CommandSystem.Commands.RemoteAdmin;
using GameCore;
using HarmonyLib;
using MapGeneration;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PluginAPI.Core;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Global;
using Toji.Patches.API.Extensions;
using Utils;
using Log = PluginAPI.Core.Log;

namespace Toji.Patches.Generic.Admins.Forces
{
    [HarmonyPatch(typeof(ForceRoleCommand), nameof(ForceRoleCommand.Execute))]
    public static class ForceclassPatch
    {
        private static readonly Dictionary<string, int> _usings;
        private static readonly Dictionary<string, int> _usingsScp;

        static ForceclassPatch()
        {
            _usings = new();
            _usingsScp = new();
        }

        private static bool Prefix(ForceRoleCommand __instance, ArraySegment<string> arguments, ICommandSender sender, out string response, ref bool __result)
        {
            __result = false;

            if (!ReferenceHub.TryGetHostHub(out ReferenceHub hub))
            {
                response = "You are not connected to a server.";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "To execute this command provide at least 2 arguments!\nUsage: " + arguments.Array[0] + " " + __instance.DisplayCommandUsage();
                return false;
            }

            ReferenceHub senderHub = (sender as PlayerCommandSender).ReferenceHub;

            List<ReferenceHub> list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out var array, false);
            var self = list.Count == 1 && senderHub == list[0];

            if (!__instance.TryParseRole(array[0], out PlayerRoleBase roleBase))
            {
                response = "Invalid role ID / name.";
                return false;
            }

            if (!__instance.HasPerms(roleBase.RoleTypeId, self, sender, out response))
                return false;

            __instance.ProvideRoleFlag(array, out RoleSpawnFlags flags);
            RoleTypeId role = roleBase.RoleTypeId;

            if (!Player.TryGet(senderHub, out Player player))
                return true;

            if (player.IsDonator(out var tag))
            {
                if (!Round.IsRoundStarted)
                {
                    response = "Дождитесь начала раунда!";
                    return false;
                }

                if (flags != RoleSpawnFlags.All && (int)flags != 3)
                {
                    response = "Разрешено только с флагами!";
                    return false;
                }

                if (role is RoleTypeId.Overwatch or RoleTypeId.Filmmaker or RoleTypeId.Tutorial or RoleTypeId.Scp0492 or RoleTypeId.Scp3114)
                {
                    response = "В эту роль вам запрещено меняться!";
                    return false;
                }

                if (role == player.Role)
                {
                    response = "Вы и так играете за эту роль!";
                    return false;
                }

                Team team = role.GetTeam();
                Faction faction = team.GetFaction();

                var ntfOrChaos = team is Team.FoundationForces or Team.ChaosInsurgency && role != RoleTypeId.FacilityGuard;
                IEnumerable<Player> players = Player.GetPlayers().Where(x => x.IsAlive && x.Role.GetFaction() != faction && x.UserId != player.UserId && x.RoleBase is FpcStandardRoleBase fpcRole && !fpcRole.IsAFK);

                if (ntfOrChaos)
                {
                    if (RoundStart.RoundLength.TotalMinutes < 3)
                    {
                        response = "3 минуты с начала раунда ещё не прошло.";
                        return false;
                    }

                    if (players.Any(x => x.Zone == FacilityZone.Surface && x.RoleBase is FpcStandardRoleBase fpcRole && !fpcRole.IsAFK))
                    {
                        response = "Кто-то есть на улице и он не помечен АФК.";
                        return false;
                    }
                }
                else
                {
                    if (RoundStart.RoundLength.TotalMinutes > 10)
                    {
                        response = "Прошло 10 минут с начала раунда.";
                        return false;
                    }

                    if (Warhead.IsDetonated)
                    {
                        response = "Альфа-Боеголовка взорвана.";
                        return false;
                    }

                    if (role == RoleTypeId.FacilityGuard && players.Any(x => x.Zone == FacilityZone.Entrance && x.RoleBase is FpcStandardRoleBase fpcRole && !fpcRole.IsAFK))
                    {
                        response = "Кто-то есть в офисной зоне и он не помечен АФК.";
                        return false;
                    }

                    if (team == Team.SCPs)
                    {
                        if (RoundStart.RoundLength.TotalMinutes > 4)
                        {
                            response = "Уже прошло более четырех минут с начала раунда";
                            return false;
                        }

                        if (players.Count(x => x.Team == Team.SCPs) >= 5)
                        {
                            response = "SCP-Объектов и так 5 или более.";
                            return false;
                        }

                        if (Swap.StartScps[role] >= Swap.Slots[role])
                        {
                            response = "Такой объект уже был в раунде!!";
                            return false;
                        }

                        if (Player.GetPlayers().Count(ply => ply.Role == role) >= Swap.Slots[role])
                        {
                            response = "Все слоты за данный объект заняты!";
                            return false;
                        }
                    }
                }

                if (!_usings.ContainsKey(player.UserId))
                    _usings.Add(player.UserId, 0);

                var max = tag.GetForcesLimit();

                var remaining = max - _usings[player.UserId];

                if (_usings[player.UserId] >= max || remaining < 0)
                {
                    response = "Ты уже максимальное кол-во раз использовал донат!";
                    return false;
                }

                if (_usingsScp.TryGetValue(player.UserId, out var scp) && scp == 1)
                {
                    response = "Ты уже становился SCP в этом раунде!";
                    return false;
                }

                _usings[player.UserId]++;

                remaining--;

                response = string.Format("Вы успешно стали: {0}! {1}", role.Translate(), remaining > 0 ? $"Осталось {remaining} использований" : "Вы использовали максимальное кол-во раз!");

                __result = true;

                foreach (ReferenceHub target in list)
                {
                    if (target != null && role != RoleTypeId.Overwatch)
                    {
                        target.roleManager.ServerSetRole(role, RoleChangeReason.RemoteAdmin, flags);

                        try
                        {
                            AddLog(2, string.Format("{0} changed role of player {1} to {2}.", sender.LogName, target.LoggedNameFromRefHub(), role), 1, false);
                        }
                        catch (Exception err)
                        {
                            Log.Error($"Error on donator force: {err.Message}\n{err.StackTrace}");
                        }
                    }
                }

                if (Swap.AllowedScps.Contains(role))
                {
                    _usingsScp.Add(player.UserId, 1);

                    Swap.StartScps[role]++;

                    /*foreach (Player ply in Player.GetPlayers())
                    {
                        if (!ply.TryGetSubclass(out var subclass) || subclass.Name != "Информатор")
                            continue;

                        var text = $"Донатер стал {role.Translate()}";

                        ply.ReceiveHint($"<line-height=95%><size=95%><voffset=-20em><b><color=#FF9500>{text}</color></b></voffset></size>", 3);
                        ply.SendConsoleMessage(text, "yellow");
                    }*/
                }

                return false;
            }

            return true;
        }

        public static void Reset()
        {
            _usings.Clear();
            _usingsScp.Clear();
        }

        private static void AddLog(int module, string msg, int type, bool init = false)
        {
            var text = TimeBehaviour.Rfc3339Time();

            var lockObject = ServerLogs.LockObject;

            lock (lockObject)
            {
                ServerLogs.Queue.Enqueue(new(msg, ServerLogs.Txt[type], ServerLogs.Modulestxt[module], text));
            }

            if (init)
                return;

            ServerLogs._state = ServerLogs.LoggingState.Write;
        }
    }
}
