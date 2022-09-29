namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {

        public static TValue GetOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key
        )
        {
            if (!dict.TryGetValue(key, out var val))
            {
                return val;
            }

            return default;
        }

        /// <summary>
        /// If a key exists in the dictionary, the corresponding value is returned.
        /// If the key is not found, the <paramref name="addFunction"/> function is
        /// executed and the result is added to the dictionary using
        /// <paramref name="key"/>.
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key,
            TValue addValue
        )
        {
            if (!dict.TryGetValue(key, out var val))
            {
                val = addValue;
                dict[key] = addValue;
            }

            return val;
        }

        /// <summary>
        /// If a key exists in the dictionary, the corresponding value is returned.
        /// If the key is not found, the <paramref name="addFunction"/> function is
        /// executed and the result is added to the dictionary using
        /// <paramref name="key"/>.
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key,
            Func<TValue> addFunction
        )
        {
            if (!dict.TryGetValue(key, out var val))
            {
                val = addFunction();
                dict[key] = val;
            }

            return val;
        }

        public static void AddOrReplace<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value
        )
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void AddOrIgnore<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value
        )
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        public static void SetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value
        )
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void AddUnion<TKey, TValue>(
            this IDictionary<TKey, HashSet<TValue>> dictionary,
            TKey key,
            TValue value
        )
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(value);
            }
            else
            {
                dictionary.Add(key, new HashSet<TValue> { value });
            }
        }
    }
}
