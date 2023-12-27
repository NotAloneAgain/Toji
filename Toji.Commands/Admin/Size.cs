using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using Toji.Commands.API.Enums;
using Toji.Commands.API.Features;
using UnityEngine;

namespace Marine.Commands.Commands
{
    public class Size : CooldownCommand
    {
        public override string Command { get; set; } = "size";

        public override string Description { get; set; } = "Команда для изменения размера.";

        public override List<CommandType> Types { get; set; } = new List<CommandType>(2) { CommandType.RemoteAdmin, CommandType.ServerConsole };

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 1, "[ЧИСЛО]" },
            { 2, "[ИГРОКИ] [ЧИСЛО]" },
            { 3, "[ВЕКТОР]" },
            { 4, "[ИГРОКИ] [ВЕКТОР]" },
        };

        public override string[] Aliases { get; set; } = new string[1] { "scale" };

        public override CommandPermission Permission { get; set; } = new(true, new HashSet<string>()
        {
            "adm",
            "modt",
            "ceo"
        }, new HashSet<string>(0));

        public override int Cooldown { get; set; } = 3;

        public override CommandResultType Handle(List<object> arguments, Player player, out string response)
        {
            if (base.Handle(arguments, player, out response) == CommandResultType.Fail)
            {
                return CommandResultType.Fail;
            }

            if (arguments.Count == 1)
            {
                player.Scale = (Vector3)arguments[0];

                return CommandResultType.Success;
            }
            else if (arguments.Count == 2)
            {
                var list = (List<Player>)arguments[0];

                if (!list.Any())
                {
                    list.Add(player);
                }

                foreach (Player ply in list)
                {
                    ply.Scale = (Vector3)arguments[1];
                }

                return CommandResultType.Success;
            }
            else
            {
                return CommandResultType.Fail;
            }
        }

        public override bool ParseSyntax(List<string> input, int count, out List<object> output)
        {
            output = new();

            if (count == 1)
            {
                if (!float.TryParse(input[0], out var value))
                {
                    return false;
                }

                output.Add(Vector3.one * value);

                return true;
            }
            else if (count == 2)
            {
                if (!TryParsePlayers(input[0], out List<Player> players) || !float.TryParse(input[1], out var value))
                {
                    return false;
                }

                output.Add(players);
                output.Add(Vector3.one * value);

                return true;
            }
            else if (count == 3)
            {
                if (!float.TryParse(input[0], out var x) || !float.TryParse(input[1], out var y) || !float.TryParse(input[2], out var z))
                {
                    return false;
                }

                output.Add(new Vector3(x, y, z));

                return true;
            }
            else if (count == 4)
            {
                if (!TryParsePlayers(input[0], out List<Player> players) || !float.TryParse(input[1], out var x) || !float.TryParse(input[2], out var y) || !float.TryParse(input[3], out var z))
                {
                    return false;
                }

                output.Add(players);
                output.Add(new Vector3(x, y, z));

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
