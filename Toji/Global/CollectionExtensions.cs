using System;
using System.Collections.Generic;
using System.Linq;

namespace Toji.Global
{
    public static class CollectionExtensions
    {
        public static IEnumerable<TData> DistinctWhere<TData>(this IEnumerable<TData> source, Func<TData, TData, bool> filter)
        {
            var result = new List<TData>();

            foreach (var element in source)
            {
                if (result.Any(data => filter(data, element)))
                {
                    continue;
                }

                result.Add(element);
            }

            return result;
        }

        public static Queue<T> Copy<T>(this Queue<T> source)
        {
            Queue<T> dest = new Queue<T>(source);

            return dest;
        }
    }
}
