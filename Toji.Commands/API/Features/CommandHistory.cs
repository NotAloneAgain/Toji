using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using Toji.Commands.API.Enums;

namespace Toji.Commands.API.Features
{
    public class CommandHistory
    {
        public Dictionary<Player, List<CommandUse>> Uses { get; set; } = [];

        public void Add(Player player, CommandUse use)
        {
            if (!IsUsedBy(player))
            {
                Uses.Add(player, [ use ]);

                return;
            }

            Uses[player].Add(use);
        }

        public bool IsUsedBy(Player player)
        {
            return Uses != null && Uses.ContainsKey(player);
        }

        public bool HasSuccessfulUse(Player player)
        {
            return GetLastSuccessfulUse(player) != null;
        }

        public CommandUse GetLastUse(Player player)
        {
            return IsUsedBy(player) ? Uses[player].Last() : null;
        }

        public CommandUse GetLastSuccessfulUse(Player player)
        {
            return IsUsedBy(player) && Uses[player].Any(use => use.Result == CommandResultType.Success) ? Uses[player].Last(use => use.Result == CommandResultType.Success) : null;
        }
    }
}
