using System;
using System.Collections.Generic;
using Utils;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class ClampedIntModifiableStatData : ClampedIntStatData
    {
        private readonly StatModifiersCollection _modifiersCollection;
        private readonly float _source;

        public ClampedIntModifiableStatData(StatObject statObject, float maxSource,
            IEnumerable<StatModifier> modifiers, float initialRatio, bool saveRatio, int minBoundsDelta = 2)
            : base(statObject, 0, minBoundsDelta, 0, false, saveRatio, minBoundsDelta)
        {
            if (maxSource <= 0 || float.IsInfinity(maxSource))
                throw new Exception("maxSource should be positive finite value");

            _modifiersCollection = new();

            foreach (var m in modifiers)
            {
                _modifiersCollection.AddModifier(m, 1);
            }

            _modifiersCollection.FinishAddingModifiers();
            var max = MathModule.ClampLongToInt((long)_modifiersCollection.ModifyValue(_source));
            SetBounds(0, max, System.Math.Clamp((int)(max * initialRatio), Min, max));

            _modifiersCollection.OnModified += HandleMaxBoundingStatModified;
        }


        private void HandleMaxBoundingStatModified(StatModifiersCollection statsCollection)
        {
            var max = MathModule.ClampLongToInt((long)_modifiersCollection.ModifyValue(_source));
            SetBounds(0, max);
        }
    }
}
