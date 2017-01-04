using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains dictionary extension methods that simplify getting valus from dictionaries and adding/updating values to dictionaries
/// without throwing exceptions or using awkward code.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Checks if the given key already exists. If it does, the value is updated.
    /// Otherwise, the value is added with the given key.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static IDictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }

        return dictionary;
    }


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

