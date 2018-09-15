using System.Collections;
using System.Collections.Generic;

namespace FastDictionaryComparer
{
    public class ComparableDictionaryAllOf<T, Y>
    {
        public Dictionary<T, Y> Value { get; }
        private int _hashCode { get; }

        internal ComparableDictionaryAllOf(Dictionary<T, Y> dict, int?[] comparableValues)
        {
            Value = dict;
            _hashCode = ((IStructuralEquatable)comparableValues).GetHashCode(EqualityComparer<int?>.Default);
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                var casted = obj as ComparableDictionaryAllOf<T, Y>;
                if (casted != null)
                {
                    return casted._hashCode == _hashCode;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public static bool operator ==(ComparableDictionaryAllOf<T, Y> obj1, ComparableDictionaryAllOf<T, Y> obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(ComparableDictionaryAllOf<T, Y> obj1, ComparableDictionaryAllOf<T, Y> obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}