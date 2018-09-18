using System;
using System.Collections.Generic;
using System.Linq;
using FastDictionaryComparer;


// TO FACTORY FUNC???
public class EquatableDictionaryFactory<T, Y>
{
    private readonly T[] _allKeys;

    public EquatableDictionaryFactory(Dictionary<T, Y>[] dicts)
    {
        _allKeys = dicts.SelectMany(d => d.Keys).Distinct().ToArray();
    }

    public EquatableDictionaryOneOf<T, Y> CreateEquatableOneOfDictionary(Dictionary<T, Y> dict)
    {
        var (d, v) = Create(dict);
        return new EquatableDictionaryOneOf<T, Y>(d, v);
    }

    public EquatableDictionaryAllOf<T, Y> CreateEquatableAllOfDictionary(Dictionary<T, Y> dict)
    {
        var (d, v) = Create(dict);
        return new EquatableDictionaryAllOf<T, Y>(d, v);
    }

    private (Dictionary<T, Y> dict, int?[] equatableValues) Create(Dictionary<T, Y> dict)
    {
        var equatableValues = _allKeys.Select(key =>
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value.GetHashCode();
            }
            return new Nullable<int>();
        }).ToArray();
        return (dict, equatableValues);
    }
}