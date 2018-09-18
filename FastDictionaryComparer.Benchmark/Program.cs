using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace FastDictionaryComparer.Benchmark
{
    [RPlotExporter, RankColumn]
    public class FastDictionaryComparerBenchmark
    {
        private static Random _random = new Random();
        private EquatableDictionaryOneOf<string, string>[] _equatableOneOfDicts;
        private EquatableDictionaryAllOf<string, string>[] _equatableAllOfDicts;
        private Dictionary<string, string>[] _data;
        private EquatableDictionaryFactory<string, string> _factory;
        private Dictionary<string, string> _toFindDict = new Dictionary<string, string> { { "12345", "12345" }, { "1234", "1234" }, { "123", "123" } };

        [Params(100, 500, 1000, 2000, 3000, 5000, 10000)]
        public int DictNumber;

        [Params(10)]
        public int KeyValueStringLength;

        [Params(10)]
        public int DictLength;

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = Enumerable.Range(0, DictNumber).Select(_ => Enumerable.Range(0, DictLength).ToDictionary(k => k.ToString(), v => RandomString(KeyValueStringLength))).ToArray();
            _factory = new EquatableDictionaryFactory<string, string>(_data);
            _equatableOneOfDicts = _data.Select(d => _factory.CreateEquatableOneOfDictionary(d)).ToArray();
            _equatableAllOfDicts = _data.Select(d => _factory.CreateEquatableAllOfDictionary(d)).ToArray();
        }

        [Benchmark]
        public int RefCount()
        {
            return _data.Count(x => x == _toFindDict);
        }

        [Benchmark]
        public int LinqCount()
        {
            return _data.Count(x =>
            {
                foreach (var kvp in x)
                {
                    if (_toFindDict.TryGetValue(kvp.Key, out var v))
                    {
                        return v == kvp.Value;
                    }
                }
                return false;
            });
        }

        [Benchmark]
        public int EquatableOneOfDictionaryCount()
        {
            var toFind = _factory.CreateEquatableOneOfDictionary(_toFindDict);
            return _equatableOneOfDicts.Count(x => x == toFind);
        }

        [Benchmark]
        public int EquatableAllOfDictionaryCount()
        {
            var toFind = _factory.CreateEquatableAllOfDictionary(_toFindDict);
            return _equatableAllOfDicts.Count(x => x == toFind);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FastDictionaryComparerBenchmark>();
        }
    }
}
