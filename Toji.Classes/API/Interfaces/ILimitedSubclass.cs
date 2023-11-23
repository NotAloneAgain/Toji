using System.Collections.Generic;

namespace Toji.Classes.API.Interfaces
{
    public interface ILimitedSubclass
    {
        HashSet<string> Groups { get; }

        HashSet<string> Users { get; }

        bool IsDonate { get; }

        bool IsAdmin { get; }
    }
}
