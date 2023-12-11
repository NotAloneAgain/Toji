using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toji.Global
{
    public static class ArrayExtensions
    {
        public static Queue<T> Copy<T>(this Queue<T> source)
        {
            Queue<T> dest = new Queue<T>(source);

            return dest;
        }
    }
}
