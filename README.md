# <img alt="logo" src="https://raw.githubusercontent.com/undernotic/FastDictionaryComparer/master/assets/logo.png">

Compare dictionaries in performant way, much faster than using linq.

## Installing

## Simple use example

```csharp
var set = Enumerable.Range(0, 10).Select(i => new Dictionary<string, string> { { "key1", i.ToString() }, { "key2", i.ToString() } });

var toFind = new Dictionary<string, string> { { "key1", "1" } };

var factory = new EquatableDictionaryFactory<string, string>(set.Concat(new[] { toFind1, toFind2, toFind3, toFind4 }).ToArray());

Assert.Contains(factory.CreateEquatableOneOfDictionary(toFind1), set.Select(factory.CreateEquatableOneOfDictionary)); // It's true
```

## Api

```csharp
public EquatableDictionaryFactory(Dictionary<T, Y>[] dicts, IEqualityComparer<T> keyEqualityComparer = null, IEqualityComparer<Y> valueEqualityComparer = null);

// EquatableDictionaryFactory methods
public EquatableDictionaryOneOf<T, Y> CreateEquatableOneOfDictionary(Dictionary<T, Y> dict);
public EquatableDictionaryAllOf<T, Y> CreateEquatableAllOfDictionary(Dictionary<T, Y> dict);
```

#### EquatableDictionaryOneOf

One of key-value pairs should match to consider two dictionaries being equal.

#### EquatableDictionaryAllOf

All of key-value pairs should match to consider two dictionaries being equal.


## Benchmark

<img alt="benchmark" src="https://raw.githubusercontent.com/undernotic/FastDictionaryComparer/master/assets/benchmark.result.png">

See [FastDictionaryComparer.Benchmark/Program.cs](https://github.com/UnderNotic/FastDictionaryComparer/blob/master/FastDictionaryComparer.Benchmark/Program.cs)

## Authors

- **Piotr Szymura** - [undernotic](https://github.com/undernotic)

See also the list of [contributors](https://github.com/undernotic/etcdgrpcclient/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE) file for details
