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
        private FastAllDictionaryComparer<string, string> allDictionaryComparer;
        private FastDictionaryComparer<string, string> dictionaryComparer;
        private ComparableDictionary<string, string>[] comparableDicts;
        private AllComparableDictionary<string, string>[] allComparableDicts;
        private Dictionary<string, string>[] data;

        [Params(1000)]
        public int DictNumber;

        [Params(5)]
        public int KeyValueStringLength;

        [Params(5)]
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
            data = Enumerable.Range(0, DictNumber).Select(_ => Enumerable.Range(0, DictLength).ToDictionary(k => k.ToString(), v => RandomString(KeyValueStringLength))).ToArray();
            allDictionaryComparer = FastAllDictionaryComparer<string, string>.Create(data);
            dictionaryComparer = FastDictionaryComparer<string, string>.Create(data);
            comparableDicts = ComparableDictionaryFactory.CreateComparableDictionaries(data);
            allComparableDicts = ComparableDictionaryFactory.CreateAllComparableDictionaries(data);
        }

        [Benchmark]
        public bool RefContains() => data.Contains(new Dictionary<string, string> { { "12345", "12345" }, { "1234", "1234" }, { "123", "123" } });

        [Benchmark]
        public bool LinqContains()
        {
            var toFind = new Dictionary<string, string> { { "12345", "12345" }, { "1234", "1234" }, { "123", "123" } };
            return data.Any(x =>
            {
                foreach (var kvp in x)
                {
                    if (toFind.TryGetValue(kvp.Key, out var v))
                    {
                        return v == kvp.Value;
                    }
                }
                return false;
            });
        }

        [Benchmark]
        public bool AllDictionaryComparerContains() => data.Contains(new Dictionary<string, string> { { "12345", "12345" }, { "1234", "1234" }, { "123", "123" } }, allDictionaryComparer);

        [Benchmark]
        public bool DictionaryComparerContains() => data.Contains(new Dictionary<string, string> { { "12345", "12345" }, { "1234", "1234" }, { "123", "123" } }, dictionaryComparer);

        [Benchmark]
        public bool ComparableDictionaryContains() => comparableDicts.Contains(new ComparableDictionary<string, string>(null, Enumerable.Range(0, DictLength).Cast<int?>().ToArray()));

        [Benchmark]
        public bool AllComparableDictionaryContains() => allComparableDicts.Contains(new AllComparableDictionary<string, string>(null, Enumerable.Range(0, DictLength).Cast<int?>().ToArray()));
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FastDictionaryComparerBenchmark>();
        }
    }
}
