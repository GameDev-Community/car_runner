namespace Utils
{
    public sealed class ClampedInt : IClampedValue
    {
        /// <summary>
        /// sender, dirty delta, final delta
        /// </summary>
        public event System.Action<ClampedInt, int, int> OnValueChanged;
        /// <summary>
        /// sender, dirty delta, final delta
        /// </summary>
        public event System.Action<ClampedInt, int, int> OnMinValueReached;
        /// <summary>
        /// sender, dirty delta, final delta
        /// </summary>
        public event System.Action<ClampedInt, int, int> OnMaxValueReached;


        private readonly int _min;
        private readonly int _max;
        private int _value;


        public ClampedInt(int min, int max, int value)
        {
            _min = min;
            _max = max;
            _value = value;
        }

        public ClampedInt(int max) : this(0, max, max)
        {
        }

        public ClampedInt(int max, int curRatio) : this(0, max, max * curRatio)
        {
        }


        public int Min => _min;
        public int Max => _max;
        public int Value => _value;

        public DynamicValue DynamicValue => (DynamicValue)Value;


        public DynamicValue TryChange(DynamicValue delta, out bool reachedMin, out bool reachedMax)
        {
            if (delta.MeasuringMode != MeasuringMode.Integer)
                throw new System.Exception($"trying to change {nameof(ClampedInt)} with " +
                    $"{nameof(DynamicValue)} measuring not in integers");

            return (DynamicValue)TryChange((int)delta, out reachedMin, out reachedMax);
        }

        public void Change(DynamicValue delta)
        {
            Change((int)delta);
        }


        /// <param name="desiredDelta">can be negative</param>
        /// <returns>available delta</returns>
        public int TryChange(int delta, out bool reachedMin, out bool reachedMax)
        {
            reachedMin = reachedMax = false;
            int safeDelta;

            if (delta > 0)
            {
                int m = _max;
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
                int m = _min;
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
        public void Change(int delta)
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
