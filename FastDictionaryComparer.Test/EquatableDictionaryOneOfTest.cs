using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FastDictionaryComparer.Test
{
    public class EquatableDictionaryOneOfTest
    {
        private readonly IEnumerable<Dictionary<string, int>> _set;

        public EquatableDictionaryOneOfTest()
        {
            _set = Enumerable.Range(0, 10).Select(i => new Dictionary<string, int> { { "key1", i }, { "key2", i } });
        }

        [Fact]
        public void EquatableDictionaryOneOfShoulContain()
        {
            var toFind1 = new Dictionary<string, int> { { "key1", 1 }, { "key3", 1 } };
            var toFind2 = new Dictionary<string, int> { { "key1", 2 }, { "key2", 999 } };
            var toFind3 = new Dictionary<string, int> { { "key1", 1 } };
            var toFind4 = new Dictionary<string, int> { { "key2", 1 } };

            var factory = new EquatableDictionaryFactory<string, int>(_set.Concat(new[] { toFind1, toFind2, toFind3, toFind4 }).ToArray());

            Assert.Contains(factory.CreateEquatableOneOfDictionary(toFind1), _set.Select(factory.CreateEquatableOneOfDictionary));
            Assert.Contains(factory.CreateEquatableOneOfDictionary(toFind2), _set.Select(factory.CreateEquatableOneOfDictionary));
            Assert.Contains(factory.CreateEquatableOneOfDictionary(toFind3), _set.Select(factory.CreateEquatableOneOfDictionary));
            Assert.Contains(factory.CreateEquatableOneOfDictionary(toFind4), _set.Select(factory.CreateEquatableOneOfDictionary));
        }

        [Fact]
        public void EquatableDictionaryOneOfShouldNotContain()
        {
            var toFind1 = new Dictionary<string, int> { { "key1", 999 } };
            var toFind2 = new Dictionary<string, int> { { "key3", 2 } };

            var factory = new EquatableDictionaryFactory<string, int>(_set.Concat(new[] { toFind1, toFind2 }).ToArray());

            Assert.DoesNotContain(factory.CreateEquatableOneOfDictionary(toFind1), _set.Select(factory.CreateEquatableOneOfDictionary));
            // Assert.DoesNotContain(factory.CreateEquatableOneOfDictionary(toFind2), _set.Select(factory.CreateEquatableOneOfDictionary));
        }
    }
}
