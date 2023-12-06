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

        public static bool SetOrAdd<TKey, TValue>(this Dictionary<TKey, IList<TValue>> dict, TKey key, TValue value)
        {
            if (dict == null || key == null || value == null)
            {
                return false;
            }

            if (dict.ContainsKey(key))
            {
                dict[key].Add(value);
            }
            else
            {
                dict.Add(key, new List<TValue>() { value });
            }

            return true;
        }
    }
}
