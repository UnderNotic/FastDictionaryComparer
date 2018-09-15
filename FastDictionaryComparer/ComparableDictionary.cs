using System.Collections.Generic;

namespace FastDictionaryComparer
{
    public class ComparableDictionary<T, Y>
    {
        public Dictionary<T, Y> Value { get; }
        public int?[] ComparableValues { get; }
        private int ComparableValue { get; }

        public ComparableDictionary(Dictionary<T, Y> dict, int?[] comparableValues)
        {
            Value = dict;
            ComparableValues = comparableValues;
        }

        public override bool Equals(object obj)
        {
            var casted = obj as ComparableDictionary<T, Y>;
            if (casted != null)
            {
                for (var i = 0; i < ComparableValues.Length; i++)
                {
                    if (casted.ComparableValues[i] == ComparableValues[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}