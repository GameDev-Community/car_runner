using DevourDev.Base.Collections.Generic;
using System;
using System.Collections.Generic;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class FloatDynamicStatData : ClampedFloatStatData, IModifiableStatData
    {
        private readonly StatModifiersCollection _modifiersCollection;
        private readonly float _source;


        public FloatDynamicStatData(StatObject statObject, float maxSource, IEnumerable<StatModifier> modifiers, float initialRatio, bool saveRatio, float minBoundsDelta = 1E-10F)
            : base(statObject, 0, minBoundsDelta, 0, false, saveRatio, minBoundsDelta)
        {
            if (maxSource <= 0 || float.IsInfinity(maxSource))
                throw new Exception("maxSource should be positive finite value");

            _source = maxSource;
            _modifiersCollection = new();

            foreach (var m in modifiers)
            {
                _modifiersCollection.AddModifier(m, 1);
            }

            _modifiersCollection.FinishAddingModifiers();
            var max = _modifiersCollection.ModifyValue(_source);
            SetBounds(0, max, max * initialRatio);

            _modifiersCollection.OnModified += HandleMaxBoundingStatModified;
        }


        public StatModifiersCollection StatModifiers => _modifiersCollection;


        private void HandleMaxBoundingStatModified(StatModifiersCollection statsCollection)
        {
            var max = _modifiersCollection.ModifyValue(_source);
            SetBounds(0, max);
        }


        public override string ToString()
        {
            return $"{GetType()}, min: {Min}, max: {Max}, Value: {Value}";
        }
    }
}
