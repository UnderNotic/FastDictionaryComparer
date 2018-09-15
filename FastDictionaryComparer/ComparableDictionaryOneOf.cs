using System.Collections.Generic;

namespace FastDictionaryComparer
{
    public class ComparableDictionaryOneOf<T, Y>
    {
        public Dictionary<T, Y> Value { get; }
        private int?[] _comparableValues { get; }

        public ComparableDictionaryOneOf(Dictionary<T, Y> dict, int?[] comparableValues)
        {
            Value = dict;
            _comparableValues = comparableValues;
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                var casted = obj as ComparableDictionaryOneOf<T, Y>;
                if (casted != null)
                {
                    for (var i = 0; i < _comparableValues.Length; i++)
                    {
                        if (casted._comparableValues[i] == _comparableValues[i])
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public static bool operator ==(ComparableDictionaryOneOf<T, Y> obj1, ComparableDictionaryOneOf<T, Y> obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(ComparableDictionaryOneOf<T, Y> obj1, ComparableDictionaryOneOf<T, Y> obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}