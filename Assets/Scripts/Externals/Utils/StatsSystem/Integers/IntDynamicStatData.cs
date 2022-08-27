using System;
using System.Collections.Generic;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public sealed class IntDynamicStatData : ClampedIntStatData, IModifiableStatData, IClampedModifiable<int>
    {
        private readonly IntModifiableStatData _maxStat;


        public IntDynamicStatData(StatObject statObject, int maxSource, IEnumerable<StatModifier> modifiers, float initialRatio, bool saveRatio, int minBoundsDelta = 2)
            : base(statObject, 0, minBoundsDelta, 0, saveRatio, minBoundsDelta)
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

        public int? MinModifiableValue { get => _maxStat.MinModifiableValue; set => _maxStat.MinModifiableValue = value; }
        public int? MaxModifiableValue { get => _maxStat.MaxModifiableValue; set => _maxStat.MaxModifiableValue = value; }


        private void HandleMaxStatChanged(IValueCallback<int> sender, int delta)
        {
            SetBounds(0, sender.Value);
        }


        public override string ToString()
        {
            return $"{GetType()}, min: {Min}, max: {Max}, Value: {Value}";
        }

        public void SetModifiableClamps(int? min, int? max)
        {
            _maxStat.SetModifiableClamps(min, max);
        }
    }
}
