using System.Collections.Generic;

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
