using Exiled.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using Toji.Commands.API.Enums;
using Toji.Commands.API.Features;

namespace Marine.Commands.Commands
{
    public class DropRagdoll : CooldownCommand
    {
        public override string Command { get; set; } = "dropragdoll";

        public override string Description { get; set; } = "Команда для сбрасывания трупов.";

        public override List<CommandType> Types { get; set; } = [ CommandType.RemoteAdmin, CommandType.ServerConsole ];

        public override string[] Aliases { get; set; } = [ "ragdoll", "dropr" ];

        public override Dictionary<int, string> Syntax { get; set; } = new Dictionary<int, string>()
        {
            { 2, "[РОЛЬ] [КОЛИЧЕСТВО]" },
            { 3, "[ИГРОК] [РОЛЬ] [КОЛИЧЕСТВО]" },
        };

        public override CommandPermission Permission { get; set; } = new(true, [

            "adm",
            "modt",
            "ceo"
        ], new(0));

        public override int Cooldown { get; set; } = 3;

        public override CommandResultType Handle(List<object> arguments, Player player, out string response)
        {
            if (base.Handle(arguments, player, out response) == CommandResultType.Fail)
            {
                return CommandResultType.Fail;
            }

            if (arguments.Count == 2)
            {
                _ = Timing.RunCoroutine(_SpawnRagdolls(player, (RoleTypeId)arguments[1], (int)arguments[2]));

                return CommandResultType.Success;
            }
            else if (arguments.Count == 3)
            {
                var list = (List<Player>)arguments[0];

                if (!list.Any())
                {
                    list.Add(player);
                }

                foreach (Player ply in list)
                {
                    _ = Timing.RunCoroutine(_SpawnRagdolls(ply, (RoleTypeId)arguments[1], (int)arguments[2]));
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
            output = [];

            if (count == 2)
            {
                if (!sbyte.TryParse(input[0], out var role) || !int.TryParse(input[1], out var itemCount) || role > 20 || role < 0)
                {
                    return false;
                }

                output.Add((RoleTypeId)role);
                output.Add(itemCount);

                return true;
            }
            else if (count == 3)
            {
                if (!TryParsePlayers(input[0], out List<Player> players) || !int.TryParse(input[1], out var role) || !int.TryParse(input[2], out var itemCount) || !Enum.IsDefined(typeof(RoleTypeId), role))
                {
                    return false;
                }

                output.Add(players);
                output.Add((RoleTypeId)role);
                output.Add(itemCount);

                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerator<float> _SpawnRagdolls(Player player, RoleTypeId role, int count)
        {
            for (var index = 0; index < count - 1; index++)
            {
                _ = Ragdoll.CreateAndSpawn(role, player.CustomName, "Ебал козу?", player.Position, player.Rotation);

                yield return Timing.WaitForSeconds(0.25f);
            }
        }
    }
}
