using Externals.Utils.Runtime;
using Externals.Utils.Valuables;

namespace Externals.Utils.StatsSystem
{
    public class FloatStatData : IStatData, IValueCallback<float>, IAmountManipulatable<float>
    {
        /// <summary>
        /// sender, delta
        /// </summary>
        public event System.Action<IValueCallback<float>, float> OnValueChanged;

        private readonly StatObject _statObject;
        private float _value;

        public FloatStatData(StatObject statObject, float initialValue)
        {
            _statObject = statObject;
            _value = initialValue;
        }


        public StatObject StatObject => _statObject;
        public float Value => _value;


        #region IOperatiable

        public void Change(float delta, bool inverse = false)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            DevourRuntimeHelpers.ThrowIfInfinityOrNaN(delta);
#endif
            if (inverse)
                delta = -delta;

            _value += delta;
            OnValueChanged?.Invoke(this, delta);
        }

        public bool CanChange(float delta, out float result)
        {
            if (!float.IsNegative(delta))
                return CanAdd(delta, out result);

            if (float.IsNegative(delta))
                return CanRemove(-delta, out result);

            result = float.NaN;
            return false;
        }

        public bool TryChange(float delta, bool inverse = false)
        {
            if (inverse)
                delta = -delta;

            if (CanChange(delta, out var result))
            {
                _value = result;
                OnValueChanged?.Invoke(this, delta);
                return true;
            }

            return false;
        }

        public void Add(float delta)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            DevourRuntimeHelpers.ThrowIfNegative(delta);
            DevourRuntimeHelpers.ThrowIfInfinityOrNaN(delta);
#endif
            _value += delta;
        }

        public bool CanAdd(float delta, out float result)
        {
            if (delta < 0 || float.IsInfinity(delta))
            {
                result = default;
                return false;
            }

            result = _value + delta;
            return float.IsFinite(result);
        }

        public bool CanRemove(float delta, out float result)
        {
            if (delta < 0 || float.IsInfinity(delta))
            {
                result = default;
                return false;
            }

            result = _value - delta;
            return float.IsFinite(result);
        }

        public bool CanSet(float value)
        {
            return value == 0 || float.IsNormal(value);
        }

        public void Remove(float delta)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            DevourRuntimeHelpers.ThrowIfNegative(delta);
            DevourRuntimeHelpers.ThrowIfInfinityOrNaN(delta);
#endif
            _value -= delta;
            OnValueChanged?.Invoke(this, delta);
        }

        public void Set(float value)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            DevourRuntimeHelpers.ThrowIfInfinityOrNaN(value);
#endif
            float delta = value - _value;
            _value = value;
            OnValueChanged?.Invoke(this, delta);
        }

        public bool TryAdd(float delta)
        {
            if (CanAdd(delta, out var r))
            {
                _value = r;
                OnValueChanged?.Invoke(this, delta);
                return true;
            }

            return false;
        }

        public bool TryRemove(float delta)
        {
            if (CanRemove(delta, out var r))
            {
                _value = r;
                OnValueChanged?.Invoke(this, delta);
                return true;
            }

            return false;
        }

        public bool TrySet(float value)
        {
            if (CanSet(value))
            {
                float delta = value - _value;
                _value = value;
                OnValueChanged?.Invoke(this, delta);
                return true;
            }

            return false;
        }
        #endregion
    }
}
