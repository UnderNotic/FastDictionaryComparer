using System;
using System.Collections.Generic;
using System.Linq;
using FastDictionaryComparer;

public class EquatableDictionaryFactory<T, Y>
{
    private readonly T[] _allKeys;
    private readonly IEqualityComparer<T> _keyEqualityComparer;
    private readonly Func<Y, int> _valueGetHashCode;

    public EquatableDictionaryFactory(Dictionary<T, Y>[] dicts, IEqualityComparer<T> keyEqualityComparer = null, IEqualityComparer<Y> valueEqualityComparer = null)
    {
        _allKeys = dicts.SelectMany(d => d.Keys).Distinct(keyEqualityComparer).OrderBy(_ => _).ToArray();
        _keyEqualityComparer = keyEqualityComparer;
        if (valueEqualityComparer == null)
        {
            _valueGetHashCode = (x) => x.GetHashCode();
        }
        else
        {
            _valueGetHashCode = (x) => valueEqualityComparer.GetHashCode(x);
        }
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
        if(_keyEqualityComparer != null){
            dict = new Dictionary<T, Y>(dict, _keyEqualityComparer);
        }
        var equatableValues = _allKeys.Select(key =>
        {
            if (dict.TryGetValue(key, out var value))
            {
                return _valueGetHashCode(value);
            }
            return new Nullable<int>();
        }).ToArray();
        return (dict, equatableValues);
    }
}