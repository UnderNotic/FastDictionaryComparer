using System;
using System.Collections.Generic;
using System.Linq;
using FastDictionaryComparer;

public class ComparableDictionaryFactory<T, Y>
{
    private readonly T[] _allKeys;

    public ComparableDictionaryFactory(Dictionary<T, Y>[] dicts)
    {
        _allKeys = dicts.SelectMany(d => d.Keys).Distinct().ToArray();
    }

    public ComparableDictionaryOneOf<T, Y> CreateComparableOneOfDictionary(Dictionary<T, Y> dict)
    {
        var (d, v) = Create(dict);
        return new ComparableDictionaryOneOf<T, Y>(d, v);
    }

    public ComparableDictionaryAllOf<T, Y> CreateComparableAllOfDictionary(Dictionary<T, Y> dict)
    {
        var (d, v) = Create(dict);
        return new ComparableDictionaryAllOf<T, Y>(d, v);
    }

    private (Dictionary<T, Y> dict, int?[] comparableValues) Create(Dictionary<T, Y> dict)
    {
        var comparableValues = _allKeys.Select(key =>
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value.GetHashCode();
            }
            return new Nullable<int>();
        }).ToArray();
        return (dict, comparableValues);
    }
}