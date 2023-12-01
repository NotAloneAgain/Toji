using System.Collections.Generic;
using Toji.Classes.API.Enums;
using Toji.Classes.API.Interfaces;

namespace Toji.Classes.API.Features
{
    public sealed class PriorityComparer : IComparer<BaseSubclass>
    {
        public int Compare(BaseSubclass x, BaseSubclass y)
        {
            var first = x is IPrioritySubclass fPriority ? fPriority.Priority : LoadPriority.Medium;
            var second = y is IPrioritySubclass sPriority ? sPriority.Priority : LoadPriority.Medium;

            int value = second.CompareTo(first);

            if (value == 0)
            {
                value = x.GetHashCode().CompareTo(y.GetHashCode());
            }

            return value == 0 ? 1 : value;
        }
    }
}
