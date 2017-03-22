using System.Collections.Generic;

namespace DDI.Shared.Extensions
{

    /// <summary>
    /// Contains dictionary extension methods that simplify getting valus from dictionaries and adding/updating values to dictionaries
    /// without throwing exceptions or using awkward code.
    /// </summary>
    public static class DictionaryExtensions
    {

        /// <summary>
        /// Look up a key in a dictionary and return its value, or the value type's default value if the
        /// key is not in the dictionary.
        /// </summary>
        public static V GetValueOrDefault<K, V>(this IDictionary<K, V> dict, K key)
        {
            V val;
            if (!dict.TryGetValue(key, out val))
            {
                val = default(V);
            }
            return val;
        }

        /// <summary>
        /// Look up a key in a dictionary and return its value, or a specified default value if the key
        /// is not in the dictionary.
        /// </summary>
        public static V GetValueOrDefault<K, V>(this IDictionary<K, V> dict, K key, V defaultValue)
        {
            V val;
            if (!dict.TryGetValue(key, out val))
            {
                val = defaultValue;
            }
            return val;
        }

    }
}

