using Exiled.API.Features;
using System.Collections.Generic;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features
{
    public abstract class GroupSubclass : BaseSubclass, IGroup
    {
        public GroupSubclass() => Players = new(100);

        public HashSet<Player> Players { get; }
    }
}
