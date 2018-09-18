using System.Collections;
using System.Collections.Generic;

namespace FastDictionaryComparer
{
    public class EquatableDictionaryAllOf<T, Y>
    {
        public Dictionary<T, Y> Value { get; }
        private int _hashCode { get; }

        internal EquatableDictionaryAllOf(Dictionary<T, Y> dict, int?[] equatableValues)
        {
            Value = dict;
            _hashCode = ((IStructuralEquatable)equatableValues).GetHashCode(EqualityComparer<int?>.Default);
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                var casted = obj as EquatableDictionaryAllOf<T, Y>;
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

        public static bool operator ==(EquatableDictionaryAllOf<T, Y> obj1, EquatableDictionaryAllOf<T, Y> obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(EquatableDictionaryAllOf<T, Y> obj1, EquatableDictionaryAllOf<T, Y> obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}