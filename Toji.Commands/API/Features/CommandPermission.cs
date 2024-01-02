using System.Collections.Generic;

namespace Toji.Commands.API.Features
{
    public class CommandPermission
    {
        public CommandPermission() { }

        public CommandPermission(bool isLimited)
        {
            IsLimited = isLimited;
        }

        public CommandPermission(bool isLimited, HashSet<string> groups, HashSet<string> users) : this(isLimited)
        {
            Groups = groups;
            Users = users;
        }

        public bool IsLimited { get; set; } = false;

        public HashSet<string> Groups { get; set; } = new (0);

        public HashSet<string> Users { get; set; } = new (0);
    }
}
