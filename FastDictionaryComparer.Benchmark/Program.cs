using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace FastDictionaryComparer.Benchmark
{
    [RankColumn]
    public class FastDictionaryComparerBenchmark
    {
        private static Random random = new Random();
        private ComparableDictionaryOneOf<string, string>[] _comparableOneOfDicts;
        private ComparableDictionaryAllOf<string, string>[] _comparableAllOfDicts;
        private Dictionary<string, string>[] _data;
        private ComparableDictionaryFactory<string, string> _factory;
        private Dictionary<string, string> _toFindDict = new Dictionary<string, string> { { "12345", "12345" }, { "1234", "1234" }, { "123", "123" } };

        [Params(100, 1000)]
        public int DictNumber;

        [Params(10)]
        public int KeyValueStringLength;

        [Params(10)]
        public int DictLength;

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [GlobalSetup]
        public void Setup()
        {
            _data = Enumerable.Range(0, DictNumber).Select(_ => Enumerable.Range(0, DictLength).ToDictionary(k => k.ToString(), v => RandomString(KeyValueStringLength))).ToArray();
            _factory = new ComparableDictionaryFactory<string, string>(_data);
            _comparableOneOfDicts = _data.Select(d => _factory.CreateComparableOneOfDictionary(d)).ToArray();
            _comparableAllOfDicts = _data.Select(d => _factory.CreateComparableAllOfDictionary(d)).ToArray();
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
        public int ComparableOneOfDictionaryCount()
        {
            var toFind = _factory.CreateComparableOneOfDictionary(_toFindDict);
            return _comparableOneOfDicts.Count(x => x == toFind);
        }

        [Benchmark]
        public int ComparableAllOfDictionaryCount()
        {
            var toFind = _factory.CreateComparableAllOfDictionary(_toFindDict);
            return _comparableAllOfDicts.Count(x => x == toFind);
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
