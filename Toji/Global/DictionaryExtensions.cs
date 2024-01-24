using System.Collections.Generic;

namespace Toji.Global
{
    public static class DictionaryExtensions
    {
        public static bool SetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict == null || key == null || value == null)
            {
                return false;
            }

            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }

            return true;
        }

        public static bool SetOrAddArr<TKey, TValue, TArray>(this Dictionary<TKey, TArray> dict, TKey key, TValue value) where TArray : IList<TValue>, new()
        {
            if (dict == null || key == null || value == null)
            {
                return false;
            }

            if (dict.TryGetValue(key, out var array))
            {
                array.Add(value);
            }
            else
            {
                dict.Add(key, [ value ]);
            }

            return true;
        }
    }
}
