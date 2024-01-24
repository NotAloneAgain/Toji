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
    public class Gravity : CommandBase
    {
        public override string Command { get; set; } = "gravity";

        public override string Description { get; set; } = "Команда для смены гравитации.";

        public override List<CommandType> Types { get; set; } = [ CommandType.RemoteAdmin, CommandType.ServerConsole ];

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 3, "[ВЕКТОР]" },
        };

        public override CommandPermission Permission { get; set; } = new()
        {
            IsLimited = true,
            Groups = [

                "owner"
            ]
        };

        public override CommandResultType Handle(List<object> arguments, Player player, out string response)
        {
            var value = (Vector3)arguments[0];

            response = $"Гравитация установлена на {value}!";

            Physics.gravity = value;

            return CommandResultType.Success;
        }

        public override bool ParseSyntax(List<string> input, int count, out List<object> output)
        {
            output = [];

            if (count == 3)
            {
                if (!float.TryParse(input[0], out var x) || !float.TryParse(input[1], out var y) || !float.TryParse(input[2], out var z))
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
