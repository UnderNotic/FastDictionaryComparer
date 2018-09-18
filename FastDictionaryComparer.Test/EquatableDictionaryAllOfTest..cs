using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FastDictionaryComparer.Test
{
    public class EquatableDictionaryAllOfTest
    {
        private readonly IEnumerable<Dictionary<string, int>> _set;

        public EquatableDictionaryAllOfTest()
        {
            _set = Enumerable.Range(0, 10).Select(i => new Dictionary<string, int> { { "key1", i }, { "key2", i } });
        }

        [Fact]
        public void EquatableDictionaryAllOfShoulContain()
        {
            var toFind = new Dictionary<string, int> { { "key1", 1 }, { "key2", 1 } };

            var factory = new EquatableDictionaryFactory<string, int>(_set.Concat(new[] { toFind }).ToArray());

            Assert.Contains(factory.CreateEquatableAllOfDictionary(toFind), _set.Select(factory.CreateEquatableAllOfDictionary));
        }

        [Fact]
        public void EquatableDictionaryAllOfShouldNotContain()
        {
            var toFind1 = new Dictionary<string, int> { { "key1", 1 } };
            var toFind2 = new Dictionary<string, int> { { "key2", 2 } };
            var toFind3 = new Dictionary<string, int> { { "key1", 1 }, { "key2", 2 } };

            var factory = new EquatableDictionaryFactory<string, int>(_set.Concat(new[] { toFind1, toFind2, toFind3 }).ToArray());

            Assert.DoesNotContain(factory.CreateEquatableAllOfDictionary(toFind1), _set.Select(factory.CreateEquatableAllOfDictionary));
            Assert.DoesNotContain(factory.CreateEquatableAllOfDictionary(toFind2), _set.Select(factory.CreateEquatableAllOfDictionary));
            Assert.DoesNotContain(factory.CreateEquatableAllOfDictionary(toFind3), _set.Select(factory.CreateEquatableAllOfDictionary));
        }
    }
}
