﻿using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Commands.API.Enums;

namespace Toji.Commands.API.Features
{
    public abstract class CommandBase : ICommand
    {
        public abstract string Command { get; set; }

        public virtual string[] Aliases { get; set; } = new string[0];

        public virtual string Description { get; set; } = string.Empty;

        public virtual Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>();

        public virtual List<CommandType> Types { get; set; } = new List<CommandType>();

        public virtual Dictionary<CommandResultType, string> Messages { get; set; } = new Dictionary<CommandResultType, string>(3)
        {
            { CommandResultType.PlayerError, "Не удалось получить игрока, вызвавшего команду." },
            { CommandResultType.PermissionError, "У вас недостаточно прав для выполнения данной команды!" },
            { CommandResultType.Error, "Во время выполнения команды произошла ошибка: {0}" },
            { CommandResultType.Syntax, "Синтаксис команды: .{0} {1}" },
            { CommandResultType.Fail, "Неудачно..." },
            { CommandResultType.Success, "Успешно!" }
        };

        public abstract CommandPermission Permission { get; set; }

        public CommandHistory History { get; set; } = new();

        public void Subscribe()
        {
            foreach (CommandType type in Types)
            {
                switch (type)
                {
                    case CommandType.RemoteAdmin:
                        {
                            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(this);

                            break;
                        }
                    case CommandType.PlayerConsole:
                        {
                            QueryProcessor.DotCommandHandler.RegisterCommand(this);

                            break;
                        }
                    case CommandType.ServerConsole:
                        {
                            GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(this);

                            break;
                        }
                }
            }
        }

        public void Unsubscribe()
        {
            foreach (CommandType type in Types)
            {
                switch (type)
                {
                    case CommandType.RemoteAdmin:
                        {
                            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(this);

                            break;
                        }
                    case CommandType.PlayerConsole:
                        {
                            QueryProcessor.DotCommandHandler.UnregisterCommand(this);

                            break;
                        }
                    case CommandType.ServerConsole:
                        {
                            GameCore.Console.singleton.ConsoleCommandHandler.UnregisterCommand(this);

                            break;
                        }
                }
            }
        }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            CommandUse use = new(DateTime.Now, CommandResultType.Error);

            if (!Syntax.TryGetValue(arguments.Count, out var syntax))
            {
                response = string.Format(Messages[CommandResultType.Syntax], Command, Syntax.First().Value);

                return false;
            }

            var list = arguments.ToList();

            if (!ParseSyntax(list, arguments.Count, out List<object> args))
            {
                response = string.Format(Messages[CommandResultType.Syntax], Command, syntax);

                return false;
            }

            try
            {
                var player = Player.Get(sender);

                if (player == null)
                {
                    use.Result = CommandResultType.PlayerError;

                    response = Messages[CommandResultType.PlayerError];

                    return false;
                }

                History.Add(player, use);

                if (!CheckPermissions(player))
                {
                    use.Result = CommandResultType.PermissionError;

                    response = Messages[CommandResultType.PermissionError];

                    return false;
                }

                use.Result = Handle(args, player, out response);

                if (string.IsNullOrEmpty(response))
                    response = Messages[use.Result];

                return use.Result == CommandResultType.Success;
            }
            catch (Exception ex)
            {
                response = string.Format(Messages[CommandResultType.Error], ex.ToString());

                return false;
            }
        }

        public abstract bool ParseSyntax(List<string> input, int count, out List<object> output);

        public virtual bool CheckPermissions(Player player)
        {
            return Permission == null || !Permission.IsLimited || Permission.Users.Any() && Permission.Users.Contains(player.UserId) || Permission.Groups.Any() && ServerStatic.PermissionsHandler._members.TryGetValue(player.UserId, out var group) && Permission.Groups.Contains(group);
        }

        public abstract CommandResultType Handle(List<object> arguments, Player player, out string response);

        protected virtual bool TryParsePlayers(string players, out List<Player> result)
        {
            switch (players)
            {
                case "all":
                    {
                        result = Player.List.ToList();

                        break;
                    }
                case "0":
                case "me":
                    {
                        result = new List<Player>(1) { };

                        break;
                    }
                default:
                    {
                        if (players.Contains("."))
                        {
                            var splitted = players.Split('.');

                            result = new(splitted.Length);

                            foreach (var data in splitted)
                            {
                                if (!Player.TryGet(data, out Player player))
                                    continue;

                                result.Add(player);
                            }
                        }
                        else
                        {
                            result = new(1);

                            if (!Player.TryGet(players, out Player player))
                                return false;

                            result.Add(player);
                        }

                        break;
                    }
            }

            return result != null;
        }
    }
}
