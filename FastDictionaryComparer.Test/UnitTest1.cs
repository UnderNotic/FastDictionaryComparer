using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FastDictionaryComparer.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var x1 = new Dictionary<string, string> { { "a", "a" } };
            var x2 = new Dictionary<string, string> { { "a", "a" }, {"b", "b"} };

         
            var comp = FastDictionaryComparer<string, string>.Create(new[] { x1, x2 });

            Console.WriteLine(new[] { x1 }.Contains(x2, comp).ToString());
        }
    }
}
