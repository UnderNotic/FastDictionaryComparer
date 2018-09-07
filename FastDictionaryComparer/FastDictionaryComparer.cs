using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FastDictionaryComparer
{
    public class FastDictionaryComparer<T, Y> : IEqualityComparer<Dictionary<T, Y>>
    {
        private Dictionary<Dictionary<T, Y>, int> dict;

        public static FastDictionaryComparer<T, Y> Create(IEnumerable<Dictionary<T, Y>> dicts, IEqualityComparer<T> eq = null, IEqualityComparer<Y> eq2 = null)
        {
            var allKeys = dicts.SelectMany(d => d.Keys).Distinct().ToArray();

            var helperDict = dicts.ToDictionary(k => k, v => ArrayGetHashCode(allKeys.Select(k => v?[k]?.GetHashCode()).ToArray()));

            return new FastDictionaryComparer<T, Y>(helperDict);
        }

        private static int ArrayGetHashCode(int?[] arr)
        {
            return ((IStructuralEquatable)arr).GetHashCode(EqualityComparer<int?>.Default);
        }

        private FastDictionaryComparer(Dictionary<Dictionary<T, Y>, int> helperDict)
        {
            dict = helperDict;
        }

        public bool Equals(Dictionary<T, Y> x, Dictionary<T, Y> y)
        {
            return dict[x] == dict[y];
        }

        public int GetHashCode(Dictionary<T, Y> obj)
        {
            return 1;
        }
    }
}
