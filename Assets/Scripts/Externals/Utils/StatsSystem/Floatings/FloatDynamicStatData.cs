using System;
using System.Collections.Generic;
using UnityEngine;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class FloatDynamicStatData : ClampedFloatStatData, IModifiableStatData, IClampedModifiable<float>
    {
        private readonly FloatModifiableStatData _maxStat;


        public FloatDynamicStatData(StatObject statObject, float sourceValue, IEnumerable<StatModifier> modifiers, float initialRatio, bool saveRatio, float minBoundsDelta = 1E-10F)
            : base(statObject, 0, minBoundsDelta, 0, saveRatio, minBoundsDelta)
        {
            if (sourceValue <= 0 || float.IsInfinity(sourceValue))
                throw new Exception("maxSource should be positive finite value");

            _maxStat = new(statObject, sourceValue, modifiers);
            var max = _maxStat.Value;
            initialRatio = System.Math.Clamp(initialRatio, 0f, 1f);
            SetBounds(0, max, max * initialRatio);

            _maxStat.OnValueChanged += HandleMaxStatChanged;
        }


        public StatModifiersCollection StatModifiers => _maxStat.StatModifiers;

        public float? MinModifiableValue { get => _maxStat.MinModifiableValue; set => _maxStat.MinModifiableValue = value; }
        public float? MaxModifiableValue { get => _maxStat.MaxModifiableValue; set => _maxStat.MaxModifiableValue = value; }


        private void HandleMaxStatChanged(IValueCallback<float> sender, float delta)
        {
            SetBounds(0, sender.Value);
        }


        public override string ToString()
        {
            return $"{GetType()}, min: {Min}, max: {Max}, Value: {Value}";
        }

        public void SetModifiableClamps(float? min, float? max)
        {
            _maxStat.SetModifiableClamps(min, max);
        }
    }
}
