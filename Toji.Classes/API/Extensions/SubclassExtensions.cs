using Toji.Classes.API.Features;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Extensions
{
    public static class SubclassExtensions
    {
        public static bool IsGroupSubclass(this BaseSubclass subclass) => subclass is IGroup;

        public static bool IsSingleSubclass(this BaseSubclass subclass) => subclass is ISingle;
    }
}
