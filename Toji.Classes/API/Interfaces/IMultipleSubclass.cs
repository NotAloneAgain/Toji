using Exiled.API.Features;
using System;
using System.Collections.Generic;
using Toji.Classes.API.Features;

namespace Toji.Classes.API.Interfaces
{
    public interface IMultipleSubclass
    {
        HashSet<Type> Allowed { get; }

        Dictionary<Player, HashSet<BaseSubclass>> Copied { get; }
    }
}
