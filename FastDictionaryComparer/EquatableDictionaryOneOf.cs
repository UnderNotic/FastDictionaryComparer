using System.Collections.Generic;

namespace FastDictionaryComparer
{
    public class EquatableDictionaryOneOf<T, Y>
    {
        public Dictionary<T, Y> Value { get; }
        private int?[] _equatableValues { get; }

        internal EquatableDictionaryOneOf(Dictionary<T, Y> dict, int?[] equatableValues)
        {
            Value = dict;
            _equatableValues = equatableValues;
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                var casted = obj as EquatableDictionaryOneOf<T, Y>;
                if (casted != null)
                {
                    for (var i = 0; i < _equatableValues.Length; i++)
                    {
                        var value = casted._equatableValues[i];
                        if (value != null && value == _equatableValues[i])
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

        public static bool operator ==(EquatableDictionaryOneOf<T, Y> obj1, EquatableDictionaryOneOf<T, Y> obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(EquatableDictionaryOneOf<T, Y> obj1, EquatableDictionaryOneOf<T, Y> obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}