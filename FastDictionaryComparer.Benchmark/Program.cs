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
        private Dictionary<string, string>[] data;

        [Params(1000, 10000)]
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
            data = Enumerable.Range(0, DictNumber).Select(_ => Enumerable.Range(0, DictLength).ToDictionary(k => RandomString(KeyValueStringLength), v => RandomString(KeyValueStringLength))).ToArray();
            allDictionaryComparer = FastAllDictionaryComparer<string, string>.Create(data);
        }

        [Benchmark]
        public bool Contains() => data.Contains(new Dictionary<string, string> { { "12345", "12345" }, { "1234", "1234" }, { "123", "123" } });
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<FastDictionaryComparerBenchmark>();
        }
    }
}
