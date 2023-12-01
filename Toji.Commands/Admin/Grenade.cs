using Exiled.API.Features;
using Exiled.API.Features.Items;
using System.Collections.Generic;
using System.Linq;
using Toji.Commands.API.Enums;
using Toji.Commands.API.Features;

namespace Marine.Commands.Commands
{
    public class Grenade : CooldownCommand
    {
        public override string Command { get; set; } = "grenade";

        public override string Description { get; set; } = "Команда для взрыва.";

        public override List<CommandType> Types { get; set; } = new List<CommandType>(2) { CommandType.RemoteAdmin, CommandType.ServerConsole };

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 1, "[ИГРОК]" },
            { 2, "[ИГРОК] [ВРЕМЯ АКТИВАЦИИ]" }
        };

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

            var list = (List<Player>)arguments[0];

            if (!list.Any())
            {
                list.Add(player);
            }

            foreach (Player ply in list)
            {
                var grenade = ExplosiveGrenade.Create(ItemType.GrenadeHE, ply) as ExplosiveGrenade;

                if (arguments.Count == 2)
                {
                    grenade.FuseTime = (float)arguments[1];
                }

                _ = grenade.SpawnActive(ply.Position, ply);
            }

            return CommandResultType.Success;
        }

        public override bool ParseSyntax(List<string> input, int count, out List<object> output)
        {
            output = new();

            if (!TryParsePlayers(input[0], out List<Player> players))
            {
                return false;
            }

            output.Add(players);

            if (count == 2)
            {
                if (!float.TryParse(input[1], out var value))
                {
                    return false;
                }

                output.Add(value);

                return true;
            }

            return true;
        }
    }
}
