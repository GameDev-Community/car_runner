using System;
using System.Collections.Generic;
using UnityEngine;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class FloatDynamicStatData : ClampedFloatStatData, IModifiableStatData
    {
        private readonly FloatModifiableStatData _maxStat;


        public FloatDynamicStatData(StatObject statObject, float maxSource, IEnumerable<StatModifier> modifiers, float initialRatio, bool saveRatio, float minBoundsDelta = 1E-10F)
            : base(statObject, 0, minBoundsDelta, 0, false, saveRatio, minBoundsDelta)
        {
            if (maxSource <= 0 || float.IsInfinity(maxSource))
                throw new Exception("maxSource should be positive finite value");

            _maxStat = new(statObject, maxSource, modifiers);
            var max = _maxStat.Value;
            SetBounds(0, max, max * initialRatio);

            _maxStat.OnFloatValueChanged += HandleMaxStatChanged;
        }


        public StatModifiersCollection StatModifiers => _maxStat.StatModifiers;


        public Vector2? MaxBoundClamps { get => _maxStat.Clamps; set => _maxStat.Clamps = value; }


        private void HandleMaxStatChanged(IFloatValueCallback sender, float delta)
        {
            SetBounds(0, sender.Value);
        }


        public override string ToString()
        {
            return $"{GetType()}, min: {Min}, max: {Max}, Value: {Value}";
        }
    }
}
