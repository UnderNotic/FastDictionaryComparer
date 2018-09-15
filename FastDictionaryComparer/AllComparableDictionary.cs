using System.Collections;
using System.Collections.Generic;

namespace FastDictionaryComparer
{
    public class AllComparableDictionary<T, Y>
    {
        public Dictionary<T, Y> Value { get; }
        private int HashCode { get; }

        public AllComparableDictionary(Dictionary<T, Y> dict, int?[] comparableValues)
        {
            Value = dict;
            HashCode = ((IStructuralEquatable)comparableValues).GetHashCode(EqualityComparer<int?>.Default);
        }

        public override bool Equals(object obj)
        {
            var casted = obj as AllComparableDictionary<T, Y>;
            if (casted != null)
            {
               return casted.HashCode == HashCode;
               
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode;
        }
    }
}