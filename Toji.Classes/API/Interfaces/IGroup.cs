using Exiled.API.Features;
using System.Collections.Generic;

namespace Toji.Classes.API.Interfaces
{
    public interface IGroup
    {
        HashSet<Player> Players { get; }
    }
}
