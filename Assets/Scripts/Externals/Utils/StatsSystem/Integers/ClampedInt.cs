using Externals.Utils.Valuables;
using System;
using UnityEngine;
using Utils;

namespace Externals.Utils.StatsSystem
{
    //todo: обработать границы int32
    public class ClampedInt : IValueCallback<int>, IAmountManipulatable<int>, IClampedAmountManipulatable<int>, IClampedBoundsManipulatable<int>
    {
        /// <summary>
        /// sender, delta
        /// </summary>
        public event System.Action<IValueCallback<int>, int> OnValueChanged;

        /// <summary>
        /// sender, dirty delta, safe delta
        /// </summary>
        public event System.Action<ClampedInt, int, int> OnClampedValueChanged;
        public event System.Action<ClampedInt, int, int> OnClampedValueReachedMin;
        public event System.Action<ClampedInt, int, int> OnClampedValueReachedMax;

        /// <summary>
        /// sender, old bounds (min, max, cur)
        /// </summary>
        public event System.Action<ClampedInt, Vector3Int> OnBoundsChanged;

        private int _min;
        private int _max;
        private int _value;

        private bool _saveRatio;

        public ClampedInt(int min, int max, int initial, bool saveRatio, int minBoundsDelta = 2)
        {

            _saveRatio = saveRatio;
            MinBoundsDelta = minBoundsDelta;
            _min = min;
            _max = max;
            int boundsDelta = _max - _min;
            int boundsCheck = minBoundsDelta - boundsDelta;

            if (boundsCheck > 0)
                _max += boundsCheck;

            _value = System.Math.Clamp(initial, _min, _max);
        }


        public int Min => _min;
        public int Max => _max;
        public int Value => _value;


        public int MinBoundsDelta { get; set; }
        public bool SaveRatio { get => _saveRatio; set => _saveRatio = value; }



        public void SetBounds(int newMin, int newMax, int newCur)
        {
            Vector3Int oldBounds = new(_min, _max, _value);
            int boundsDelta = newMax - newMin;
            int boundsCheck = MinBoundsDelta - boundsDelta;

            if (boundsCheck > 0)
                newMax += boundsCheck;

            _min = newMin;
            _max = newMax;
            _value = MathModule.Clamp(newCur, _min, _max, out var rmin, out var rmax);
            OnBoundsChanged?.Invoke(this, oldBounds);
            SetValue(_value, newCur - oldBounds.z, _value - oldBounds.z, rmin, rmax);
        }

        public void SetBounds(int newMin, int newMax)
        {
            Vector3Int oldBounds = new(_min, _max, _value);
            int boundsDelta = newMax - newMin;
            int boundsCheck = MinBoundsDelta - boundsDelta;

            if (boundsCheck > 0)
                newMax += boundsCheck;

            _min = newMin;
            _max = newMax;
            bool rmin, rmax;
            int newCur;

            if (!_saveRatio)
            {
                newCur = MathModule.Clamp(_value, _min, _max, out rmin, out rmax);
            }
            else
            {
                rmin = rmax = false;
                newCur = MathModule.SaveRatio(_value, oldBounds.x, oldBounds.y, _min, _max);
            }

            OnBoundsChanged?.Invoke(this, oldBounds);
            int delta = newCur - oldBounds.z;
            SetValue(_value, delta, delta, rmin, rmax);
        }


        #region amount manipulatable
        public bool CanChange(int delta, out int result)
        {
            if (delta > 0)
                return CanAdd(delta, out result);

            if (delta < 0)
                return CanRemove(-delta, out result);

            result = _value;
            return true;
        }

        public bool CanAdd(int delta, out int result)
        {
            if (delta < 0)
            {
                result = default;
                return false;
            }

            result = MathModule.ClampLongToInt((long)_value + delta);
            return true;
        }

        public bool CanRemove(int delta, out int result)
        {
            if (delta < 0)
            {
                result = default;
                return false;
            }

            result = MathModule.ClampLongToInt((long)_value - delta);
            return true;
        }

        public bool CanSet(int value)
        {
            return true;
        }

        public void Change(int delta, bool inverse = false)
        {
            if (inverse)
                delta = -delta;

            var v = MathModule.ClampLongToInt((long)_value + delta);
            Set(v);
        }

        public void Add(int delta)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (delta < 0)
                throw new Exception("value should be positive");
#endif
            long lv = _value + delta;
            bool rmax = lv >= _max;

            if (rmax)
                lv = _max;

            var v = (int)lv;
            SetValue(v, delta, v - _value, false, rmax);
        }

        public void Remove(int delta)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (delta < 0)
                throw new Exception("value should be positive");
#endif

            long lv = _value - delta;
            bool rmin = lv <= _min;

            if (rmin)
                lv = _min;

            var v = (int)lv;
            SetValue(v, -delta, v - _value, rmin, false);
        }

        public void Set(int value)
        {
            long longDelta = (long)value - _value;
            var v = MathModule.Clamp(value, _min, _max, out var rmin, out var rmax);
            int safeDelta = v - _value;
            SetValue(v, longDelta, safeDelta, rmin, rmax);
        }

        public bool TryChange(int delta, bool inverse = false)
        {
            if (inverse)
                delta = -delta;

            Change(delta);
            return true;
        }

        public bool TryAdd(int delta)
        {
            if (CanAdd(delta, out var result))
            {
                Set(result);
            }
            return false;
        }

        public bool TryRemove(int delta)
        {
            if (delta < 0)
                return false;

            Remove(delta);
            return true;
        }

        public bool TrySet(int value)
        {
            Set(value);
            return true;
        }
        #endregion

        #region clamped amount manipulatable
        public bool CanAddExact(int delta)
        {
            if (delta < 0)
                return false;

            long desired = (long)_value + delta;
            return desired <= _max && desired >= _min;
        }

        public bool CanRemoveExact(int delta)
        {
            if (delta < 0)
                return false;

            long desired = (long)_value - delta;
            return desired <= _max && desired >= _min;
        }
        #endregion

        protected void SetValue(int value, long longDirtyDelta, long longSafeDelta, bool reachedMin, bool reachedMax)
        {
            _value = value;
            int dirtyDelta = MathModule.ClampLongToInt(longDirtyDelta);
            int safeDelta = MathModule.ClampLongToInt(longSafeDelta);
            OnValueChanged?.Invoke(this, safeDelta);
            OnClampedValueChanged?.Invoke(this, dirtyDelta, safeDelta);

            if (reachedMin)
                OnClampedValueReachedMin?.Invoke(this, dirtyDelta, safeDelta);
            else if (reachedMax)
                OnClampedValueReachedMax?.Invoke(this, dirtyDelta, safeDelta);
        }


        protected void SetMinInternal(int v)
        {
            _min = v;
        }

        protected void SetMaxInternal(int v)
        {
            _max = v;
        }

        protected void SetValueInternal(int v)
        {
            _value = v;
        }


        public bool CanChangeExact(int delta)
        {
            if (delta > 0)
                return CanAddExact(delta);

            if (delta < 0)
                return CanRemoveExact(-delta);

            if (delta == 0)
                return true;

            return false;
        }

        public int AddSafe(int delta)
        {
            if (delta <= 0)
                return 0;

            int canAdd = _max - _value;

            if (delta < canAdd)
                canAdd = delta;

            Add(canAdd);
            return canAdd;
        }

        public int RemoveSafe(int delta)
        {
            if (delta <= 0)
                return 0;

            int canRemove = _min - _value;

            if (delta > canRemove)
                canRemove = delta;

            Remove(canRemove);
            return canRemove;
        }

        public int ChangeSafe(int delta, bool inversed = false)
        {
            if (delta == 0)
                return 0;

            if (inversed)
                delta = -delta;

            if (float.IsNegative(delta))
                return RemoveSafe(-delta);

            return AddSafe(delta);
        }

        public int SetSafe(int value)
        {
            var v = System.Math.Clamp(value, _min, _max);
            Set(v);
            return v;
        }
    }
}
