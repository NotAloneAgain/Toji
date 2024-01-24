using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using Toji.Classes.API.Extensions;
using Toji.Classes.API.Features;
using Toji.Classes.API.Features.Subclasses;
using Toji.Classes.API.Interfaces;
using Toji.Commands.API.Enums;
using Toji.Commands.API.Features;

namespace Marine.Commands.Commands
{
    public class GiveSubclass : CommandBase
    {
        public override string Command { get; set; } = "subclass";

        public override string Description { get; set; } = "Команда для выдачи подкласса.";

        public override List<CommandType> Types { get; set; } = [ CommandType.RemoteAdmin, CommandType.ServerConsole ];

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 2, "[ИГРОК] [ПОДКЛАСС]" },
        };

        public override CommandPermission Permission { get; set; } = new(true, [

            "owner"
        ], new(0));

        public override CommandResultType Handle(List<object> arguments, Player player, out string response)
        {
            var list = (List<Player>)arguments[0];

            if (!list.Any())
            {
                list.Add(player);
            }

            response = string.Empty;

            if (!BaseSubclass.TryGet((string)arguments[1], out var subclass))
            {
                response = "Не найден такой подкласс!";

                return CommandResultType.Fail;
            }

            if (subclass is SingleSubclass single && (list.Count > 1 || single.Player != null))
            {
                response = "Невозможно выдать подкласс!";

                return CommandResultType.Fail;
            }

            if (subclass is ILimitableGroup group && (list.Count + group.Players.Count > group.Max || group.Players.Count == group.Max))
            {
                response = "Невозможно выдать подкласс!";

                return CommandResultType.Fail;
            }

            foreach (Player ply in list)
            {
                if (ply.TryGetSubclass(out var sub))
                {
                    sub.Revoke(ply);
                }

                subclass.Assign(ply);
            }

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
                output.Add(input[1]);
            }

            return true;
        }
    }
}
