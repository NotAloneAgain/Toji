using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;
using Toji.Commands.API.Enums;
using Toji.Commands.API.Features;
using UnityEngine;

namespace Marine.Commands.Commands
{
    public class Position : CommandBase
    {
        public override string Command { get; set; } = "setpos";

        public override string Description { get; set; } = "tppos";

        public override List<CommandType> Types { get; set; } = [ CommandType.RemoteAdmin, CommandType.ServerConsole ];

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 2, "[ИГРОК] [ЧИСЛО]" },
            { 4, "[ИГРОК] [ВЕКТОР]" },
        };

        public override CommandPermission Permission { get; set; } = new()
        {
            IsLimited = true,
            Groups = [ "owner" ]
        };

        public override CommandResultType Handle(List<object> arguments, Player player, out string response)
        {
            var value = (Vector3)arguments[1];

            var list = (List<Player>)arguments[0];

            if (!list.Any())
            {
                list.Add(player);
            }

            foreach (var ply in list)
            {
                ply.Position = value;
            }

            response = $"Позиция успешно установлена {value}!";

            return CommandResultType.Success;
        }

        public override bool ParseSyntax(List<string> input, int count, out List<object> output)
        {
            output = [];

            if (!TryParsePlayers(input[0], out List<Player> players))
            {
                return false;
            }

            output.Add(players);

            if (count == 2)
            {
                if (!float.TryParse(input[1], out var vector))
                {
                    return false;
                }

                output.Add(Vector3.one * vector);

                return true;
            }
            else if (count == 4)
            {
                if (!float.TryParse(input[1], out var x) || !float.TryParse(input[2], out var y) || !float.TryParse(input[3], out var z))
                {
                    return false;
                }

                output.Add(new Vector3(x, y, z));

                return true;
            }

            return false;
        }
    }
}
