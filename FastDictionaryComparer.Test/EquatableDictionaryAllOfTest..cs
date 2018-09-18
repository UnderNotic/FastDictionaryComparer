using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FastDictionaryComparer.Test
{
    public class EquatableDictionaryAllOfTest
    {
        private readonly IEnumerable<Dictionary<string, string>> _set;

        public EquatableDictionaryAllOfTest()
        {
            _set = Enumerable.Range(0, 10).Select(i => new Dictionary<string, string> { { "key1", i.ToString() }, { "key2", i.ToString() } });
        }

        [Fact]
        public void EquatableDictionaryAllOfShouldContain()
        {
            var toFind = new Dictionary<string, string> { { "key1", "1" }, { "key2", "1" } };

            var factory = new EquatableDictionaryFactory<string, string>(_set.Concat(new[] { toFind }).ToArray());

            Assert.Contains(factory.CreateEquatableAllOfDictionary(toFind), _set.Select(factory.CreateEquatableAllOfDictionary));
        }

        [Fact]
        public void EquatableDictionaryAllOfShouldNotContain()
        {
            var toFind1 = new Dictionary<string, string> { { "key1", "1" } };
            var toFind2 = new Dictionary<string, string> { { "key2", "2" } };
            var toFind3 = new Dictionary<string, string> { { "key1", "1" }, { "key2", "2" } };

            var factory = new EquatableDictionaryFactory<string, string>(_set.Concat(new[] { toFind1, toFind2, toFind3 }).ToArray());

            Assert.DoesNotContain(factory.CreateEquatableAllOfDictionary(toFind1), _set.Select(factory.CreateEquatableAllOfDictionary));
            Assert.DoesNotContain(factory.CreateEquatableAllOfDictionary(toFind2), _set.Select(factory.CreateEquatableAllOfDictionary));
            Assert.DoesNotContain(factory.CreateEquatableAllOfDictionary(toFind3), _set.Select(factory.CreateEquatableAllOfDictionary));
        }

        [Fact]
        public void EquatableDictionaryAllOfShouldUseKeyEqualityComparer()
        {
            var toFind = new Dictionary<string, string> { { "Key1", "1" }, { "KEY2", "1" } };

            var factory = new EquatableDictionaryFactory<string, string>(_set.Concat(new[] { toFind }).ToArray(), keyEqualityComparer: StringComparer.InvariantCultureIgnoreCase);

            Assert.Contains(factory.CreateEquatableAllOfDictionary(toFind), _set.Select(factory.CreateEquatableAllOfDictionary));
        }

        [Fact]
        public void EquatableDictionaryAllOfShouldUseValueEqualityComparer()
        {
            var toFind = new Dictionary<string, string> { { "key1", "a" }, { "key2", "a" } };

            var factory = new EquatableDictionaryFactory<string, string>(_set.Concat(new[] { toFind }).ToArray(), valueEqualityComparer: StringComparer.InvariantCultureIgnoreCase);

            Assert.Contains(factory.CreateEquatableAllOfDictionary(toFind), _set.Select(factory.CreateEquatableAllOfDictionary).Concat(new[] { factory.CreateEquatableAllOfDictionary(new Dictionary<string, string> { { "key1", "A" }, { "key2", "A" } }) }));
        }
    }
}
