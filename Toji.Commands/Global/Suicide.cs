using Exiled.API.Features;
using System.Collections.Generic;
using Toji.Classes.API.Extensions;
using Toji.Commands.API.Enums;
using Toji.Commands.API.Features;

namespace Toji.Commands.Global
{
    public sealed class Suicide : CommandBase
    {
        public override string Command { get; set; } = "suicide";

        public override string Description { get; set; } = "Команда для суицида.";

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 0, string.Empty }
        };

        public override List<CommandType> Types { get; set; } = new List<CommandType>(1) { CommandType.PlayerConsole };

        public override CommandPermission Permission { get; set; } = new()
        {
            IsLimited = false,
        };

        public override bool ParseSyntax(List<string> input, int count, out List<object> output)
        {
            output = new List<object>();

            return true;
        }

        public override CommandResultType Handle(List<object> arguments, Player player, out string response)
        {
            response = string.Empty;

            if (!player.IsAlive || !player.IsHuman || player.IsTutorial)
            {
                response = "Суицид - не выход.";

                return CommandResultType.Fail;
            }

            if (player.TryGetSubclass(out var sub))
            {
                sub.Revoke(player);
            }

            player.Kill("Вскрылся...");

            return CommandResultType.Success;
        }
    }
}
