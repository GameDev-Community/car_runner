using System;
using System.Collections.Generic;
using UnityEngine;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class IntDynamicStatData : ClampedIntStatData, IModifiableStatData
    {
        private readonly IntModifiableStatData _maxStat;


        public IntDynamicStatData(StatObject statObject, int maxSource, IEnumerable<StatModifier> modifiers, float initialRatio, bool saveRatio, int minBoundsDelta = 2)
            : base(statObject, 0, minBoundsDelta, 0, false, saveRatio, minBoundsDelta)
        {
            if (maxSource <= 0 || float.IsInfinity(maxSource))
                throw new Exception("maxSource should be positive finite value");

            _maxStat = new(statObject, maxSource, modifiers);
            var max = _maxStat.Value;
            initialRatio = System.Math.Clamp(initialRatio, 0, 1);
            SetBounds(0, max, (int)(max * initialRatio));

            _maxStat.OnValueChanged += HandleMaxStatChanged;
        }


        public StatModifiersCollection StatModifiers => _maxStat.StatModifiers;


        public Vector2Int? MaxBoundClamps { get => _maxStat.Clamps; set => _maxStat.Clamps = value; }


        private void HandleMaxStatChanged(IValueCallback<int> sender, int delta)
        {
            SetBounds(0, sender.Value);
        }


        public override string ToString()
        {
            return $"{GetType()}, min: {Min}, max: {Max}, Value: {Value}";
        }
    }
}
