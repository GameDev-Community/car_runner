namespace Utils
{
    public sealed class ClampedFloat : IClampedValue
    {
        /// <summary>
        /// sender, dirty delta, final delta
        /// </summary>
        public event System.Action<ClampedFloat, float, float> OnValueChanged;
        /// <summary>
        /// sender, dirty delta, final delta
        /// </summary>
        public event System.Action<ClampedFloat, float, float> OnMinValueReached;
        /// <summary>
        /// sender, dirty delta, final delta
        /// </summary>
        public event System.Action<ClampedFloat, float, float> OnMaxValueReached;


        private readonly float _min;
        private readonly float _max;
        private float _value;


        public ClampedFloat(float min, float max, float value)
        {
            _min = min;
            _max = max;
            _value = value;
        }

        public ClampedFloat(float max) : this(0, max, max)
        {
        }

        public ClampedFloat(float max, float curRatio) : this(0, max, max * curRatio)
        {
        }


        public float Min => _min;
        public float Max => _max;
        public float Value => _value;

        public DynamicValue DynamicValue => (DynamicValue)Value;


        public DynamicValue TryChange(DynamicValue delta, out bool reachedMin, out bool reachedMax)
        {
            if (delta.MeasuringMode != MeasuringMode.Float)
                throw new System.Exception($"trying to change {nameof(ClampedInt)} with " +
                    $"{nameof(DynamicValue)} measuring not in floatings");

            return (DynamicValue)TryChange((float)delta, out reachedMin, out reachedMax);
        }

        public void Change(DynamicValue delta)
        {
            Change((float)delta);
        }


        /// <param name="desiredDelta">can be negative</param>
        /// <returns>available delta</returns>
        public float TryChange(float delta, out bool reachedMin, out bool reachedMax)
        {
            reachedMin = reachedMax = false;
            float safeDelta;

            if (delta > 0)
            {
                float m = _max;
                safeDelta = m - _value;

                if (safeDelta > delta)
                {
                    safeDelta = delta;
                }
                else
                {
                    reachedMax = true;
                    _value = m;
                }
            }
            else
            {
                float m = _min;
                safeDelta = m - _value;

                if (safeDelta < delta)
                {
                    safeDelta = delta;
                }
                else
                {
                    reachedMin = true;
                    _value = m;
                }
            }

            return safeDelta;
        }

        /// <param name="delta">can be negative</param>
        public void Change(float delta)
        {
            var safeDelta = TryChange(delta, out var reachedMin,
                out var reachedMax);

            _value += safeDelta;

            OnValueChanged?.Invoke(this, delta, safeDelta);

            if (reachedMin)
                OnMinValueReached?.Invoke(this, delta, safeDelta);

            if (reachedMax)
                OnMaxValueReached?.Invoke(this, delta, safeDelta);
        }
    }
}
