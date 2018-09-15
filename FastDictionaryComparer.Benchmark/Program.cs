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
        private ComparableDictionaryOneOf<string, string>[] comparableOneOfDicts;
        private ComparableDictionaryAllOf<string, string>[] comparableAllOfDicts;
        private Dictionary<string, string>[] data;

        [Params(1000)]
        public int DictNumber;

        [Params(5, 10)]
        public int KeyValueStringLength;

        [Params(5, 10)]
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
            comparableOneOfDicts = ComparableDictionaryFactory.CreateComparableOneOfDictionaries(data);
            comparableAllOfDicts = ComparableDictionaryFactory.CreateComparableAllOfDictionaries(data);
        }

        [Benchmark]
        public int RefCount() => data.Count(x => x == new Dictionary<string, string> { { "12345", "12345" }, { "1234", "1234" }, { "123", "123" } });

        [Benchmark]
        public int LinqCount()
        {
            var toFind = new Dictionary<string, string> { { "12345", "12345" }, { "1234", "1234" }, { "123", "123" } };
            return data.Count(x =>
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
        public int ComparableOneOfDictionaryContains() => comparableOneOfDicts.Count(x => x == new ComparableDictionaryOneOf<string, string>(null, Enumerable.Range(0, DictLength).Cast<int?>().ToArray()));

        [Benchmark]
        public int AllComparableAllOfDictionaryContains() => comparableAllOfDicts.Count(x => x == new ComparableDictionaryAllOf<string, string>(null, Enumerable.Range(0, DictLength).Cast<int?>().ToArray()));
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FastDictionaryComparerBenchmark>();
        }
    }
}
