using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toji.Classes.API.Enums
{
    public enum LoadPriority : byte
    {
        Last = 0,
        Lowest = 50,
        Low = 100,
        Medium = 128,
        High = 169,
        Highest = 228,
        First = 255
    }
}
