using System;
using System.Collections.Generic;
using System.Linq;
using FastDictionaryComparer;

public static class ComparableDictionaryFactory
{
    public static ComparableDictionaryOneOf<T, Y>[] CreateComparableOneOfDictionaries<T, Y>(Dictionary<T, Y>[] dicts) => Create(dicts).Select(t => new ComparableDictionaryOneOf<T, Y>(t.dict, t.comparableValues)).ToArray();

    public static ComparableDictionaryAllOf<T, Y>[] CreateComparableAllOfDictionaries<T, Y>(Dictionary<T, Y>[] dicts) => Create(dicts).Select(t => new ComparableDictionaryAllOf<T, Y>(t.dict, t.comparableValues)).ToArray();

    public static IEnumerable<(Dictionary<T, Y> dict, int?[] comparableValues)> Create<T, Y>(Dictionary<T, Y>[] dicts)
    {
        var allKeys = dicts.SelectMany(d => d.Keys).Distinct().ToArray();

        return dicts.Select(dict =>
        {
            var comparableValues = allKeys.Select(key =>
            {
                if (dict.TryGetValue(key, out var value))
                {
                    return value.GetHashCode();
                }
                return new Nullable<int>();
            }).ToArray();
            return (dict, comparableValues);
        });
    }
}