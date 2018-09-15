using System;
using System.Collections.Generic;
using System.Linq;
using FastDictionaryComparer;

public static class ComparableDictionaryFactory
{
    public static ComparableDictionary<T, Y>[] CreateComparableDictionaries<T, Y>(Dictionary<T, Y>[] dicts)
    {
        var allKeys = dicts.SelectMany(d => d.Keys).Distinct().ToArray();

        return dicts.Select(dict =>
        {
            var comparableValue = allKeys.Select(key =>
            {
                if (dict.TryGetValue(key, out var value))
                {
                    return value.GetHashCode();
                }
                return new Nullable<int>();
            }).ToArray();
            return new ComparableDictionary<T ,Y>(dict, comparableValue);

        }).ToArray();
    }
    public static AllComparableDictionary<T, Y>[] CreateAllComparableDictionaries<T, Y>(Dictionary<T, Y>[] dicts)
    {
        var allKeys = dicts.SelectMany(d => d.Keys).Distinct().ToArray();

        return dicts.Select(dict =>
        {
            var comparableValue = allKeys.Select(key =>
            {
                if (dict.TryGetValue(key, out var value))
                {
                    return value.GetHashCode();
                }
                return new Nullable<int>();
            }).ToArray();
            return new AllComparableDictionary<T ,Y>(dict, comparableValue);

        }).ToArray();
    }
    
}