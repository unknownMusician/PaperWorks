using System;
using UnityEngine;

namespace UM.Util
{
    [Serializable]
    public struct Range<TRangeable>
    {
        [field: SerializeField] public TRangeable Min { get; private set; }
        [field: SerializeField] public TRangeable Max { get; private set; }
        [field: SerializeField] public bool IsBounded { get; private set; }

        public Range(TRangeable min, TRangeable max)
        {
            Min = min;
            Max = max;
            IsBounded = true;
        }

        public static implicit operator Range<TRangeable>((TRangeable min, TRangeable max) tuple)
        {
            return new Range<TRangeable>(tuple.min, tuple.max);
        }
    
        public void Deconstruct(out TRangeable min, out TRangeable max)
        {
            min = Min;
            max = Max;
        }
    }
}
