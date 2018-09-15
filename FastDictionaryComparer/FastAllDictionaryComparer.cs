using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FastDictionaryComparer
{
    public class FastAllDictionaryComparer<T, Y> : IEqualityComparer<Dictionary<T, Y>>
    {
        private Dictionary<Dictionary<T, Y>, int> dict;

        public static FastAllDictionaryComparer<T, Y> Create(IEnumerable<Dictionary<T, Y>> dicts, IEqualityComparer<T> eq = null, IEqualityComparer<Y> eq2 = null)
        {
            var allKeys = dicts.SelectMany(d => d.Keys).Distinct().ToArray();

            var helperDict = dicts.ToDictionary(k => k, v => ArrayGetHashCode(allKeys.Select(k =>
            {
                if (v.TryGetValue(k, out var value))
                {
                    return value.GetHashCode();
                }
                return new Nullable<int>();
            }).ToArray()));

            return new FastAllDictionaryComparer<T, Y>(helperDict);
        }

        private static int ArrayGetHashCode(int?[] arr)
        {
            return ((IStructuralEquatable)arr).GetHashCode(EqualityComparer<int?>.Default);
        }

        private FastAllDictionaryComparer(Dictionary<Dictionary<T, Y>, int> helperDict)
        {
            dict = helperDict;
        }

        public bool Equals(Dictionary<T, Y> x, Dictionary<T, Y> y)
        {
            if (dict.TryGetValue(x, out var v1) && dict.TryGetValue(y, out var v2))
            {
                return v1 == v2;
            }
            return false;
        }

    public int GetHashCode(Dictionary<T, Y> obj)
    {
        return 1;
    }
}
}
