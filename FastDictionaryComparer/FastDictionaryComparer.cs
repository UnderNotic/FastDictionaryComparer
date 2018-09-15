using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FastDictionaryComparer
{
    public class FastDictionaryComparer<T, Y> : IEqualityComparer<Dictionary<T, Y>>
    {
        private Dictionary<Dictionary<T, Y>, int?[]> dict;
        private int dictLength;

        public static FastDictionaryComparer<T, Y> Create(IEnumerable<Dictionary<T, Y>> dicts, IEqualityComparer<T> eq = null, IEqualityComparer<Y> eq2 = null)
        {
            var allKeys = dicts.SelectMany(d => d.Keys).Distinct().ToArray();

            var helperDict = dicts.ToDictionary(k => k, v => allKeys.Select(k =>
            {
                if (v.TryGetValue(k, out var value))
                {
                    return value.GetHashCode();
                }
                return new Nullable<int>();
            }).ToArray());

            return new FastDictionaryComparer<T, Y>(helperDict);
        }

        private FastDictionaryComparer(Dictionary<Dictionary<T, Y>, int?[]> helperDict)
        {
            dict = helperDict;
            dictLength = dict.FirstOrDefault().Value.Length;
        }

        public bool Equals(Dictionary<T, Y> x, Dictionary<T, Y> y)
        {
            for (var i = 0; i < dictLength; i++)
            {
                if (dict.TryGetValue(x, out var v1) && dict.TryGetValue(y, out var v2))
                {
                    return v1[i] == v2[i];
                }
            }
            return false;
        }

        public int GetHashCode(Dictionary<T, Y> obj)
        {
            return 1;
        }
    }
}
