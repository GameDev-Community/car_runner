using Externals.Utils.Runtime;
using Externals.Utils.Valuables;
using UnityEngine;
using Utils;

namespace Externals.Utils.StatsSystem
{
    public class ClampedFloat : IFloatValueCallback, IAmountManipulatable<float>, IClampedAmountManipulatable<float>
    {
        /// <summary>
        /// sender, delta
        /// </summary>
        public event System.Action<IFloatValueCallback, float> OnFloatValueChanged;

        /// <summary>
        /// sender, dirty delta, safe delta
        /// </summary>
        public event System.Action<ClampedFloat, float, float> OnClampedValueChanged;
        /// <summary>
        /// sender, dirty delta, safe delta
        /// </summary>
        public event System.Action<ClampedFloat, float, float> OnClampedValueReachedMin;
        /// <summary>
        /// sender, dirty delta, safe delta
        /// </summary>
        public event System.Action<ClampedFloat, float, float> OnClampedValueReachedMax;

        /// <summary>
        /// sender, old bounds (min, max, cur)
        /// </summary>
        public event System.Action<ClampedFloat, Vector3> OnBoundsChanged;

        private float _min;
        private float _max;
        private float _value;

        private bool _saveRatio;


        public ClampedFloat(float min, float max, float initial, bool initialValueIsRatio, bool saveRatio, float minBoundsDelta = 1e-10f)
        {

            _saveRatio = saveRatio;
            MinBoundsDelta = minBoundsDelta;
            _min = min;
            _max = max;
            float boundsDelta = _max - _min;
            float boundsCheck = minBoundsDelta - boundsDelta;

            if (boundsCheck > 0)
                _max += boundsCheck;

            _value = initialValueIsRatio ? (_max - _min) * initial : initial;
        }


        public float Min => _min;
        public float Max => _max;
        public float Value => _value;


        public float MinBoundsDelta { get; set; }
        public bool SaveRatio { get => _saveRatio; set => _saveRatio = value; }



        public void SetBounds(float newMin, float newMax, float newCur)
        {
            Vector3 oldBounds = new(_min, _max, _value);
            float boundsDelta = newMax - newMin;
            float boundsCheck = MinBoundsDelta - boundsDelta;

            if (boundsCheck > 0)
                newMax += boundsCheck;

            _min = newMin;
            _max = newMax;
            _value = MathModule.Clamp(newCur, _min, _max, out var rmin, out var rmax);
            OnBoundsChanged?.Invoke(this, oldBounds);
            SetValue(_value, newCur - oldBounds.z, _value - oldBounds.z, rmin, rmax);
        }

        public void SetBounds(float newMin, float newMax)
        {
            Vector3 oldBounds = new(_min, _max, _value);
            float boundsDelta = newMax - newMin;
            float boundsCheck = MinBoundsDelta - boundsDelta;

            if (boundsCheck > 0)
                newMax += boundsCheck;

            _min = newMin;
            _max = newMax;
            bool rmin, rmax;
            float newCur;

            if (_saveRatio)
            {
                rmin = rmax = false;
                newCur = MathModule.SaveRatio(_value, oldBounds.x, oldBounds.y, _min, _max);
            }
            else
            {
                newCur = MathModule.Clamp(_value, _min, _max, out rmin, out rmax);
            }

            _value = newCur;
            OnBoundsChanged?.Invoke(this, oldBounds);
            float delta = newCur - oldBounds.z;
            SetValue(newCur, delta, delta, rmin, rmax);
        }


        #region amount manipulatable
        public bool CanChange(float delta, out float result)
        {
            if (!float.IsNegative(delta))
                return CanAdd(delta, out result);

            if (float.IsNegative(delta))
                return CanRemove(-delta, out result);

            result = float.NaN;
            return false;
        }

        public bool CanAdd(float delta, out float result)
        {
            if (float.IsNegative(delta))
            {
                result = float.NaN;
                return false;
            }

            result = System.Math.Clamp(_value + delta, _min, _max);
            return true;
        }

        public bool CanRemove(float delta, out float result)
        {
            if (float.IsNegative(delta))
            {
                result = float.NaN;
                return false;
            }

            result = System.Math.Clamp(_value - delta, _min, _max);
            return true;
        }

        public bool CanSet(float value)
        {
            return !float.IsNaN(value);
        }

        public void Change(float delta, bool inverse = false)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            DevourRuntimeHelpers.ThrowIfNaN(delta);
#endif

            if (inverse)
                delta = -delta;

            Set(_value + delta);
        }

        public void Add(float delta)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            DevourRuntimeHelpers.ThrowIfNegative(delta);
            DevourRuntimeHelpers.ThrowIfNaN(delta);
#endif
            var v = _value + delta;
            bool rmax = v >= _max;

            if (rmax)
                v = _max;

            SetValue(v, delta, v - _value, false, rmax);
        }

        public void Remove(float delta)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            DevourRuntimeHelpers.ThrowIfNegative(delta);
            DevourRuntimeHelpers.ThrowIfNaN(delta);
#endif

            var v = _value - delta;
            bool rmin = v <= _min;

            if (rmin)
                v = _min;

            SetValue(v, -delta, v - _value, rmin, false);
        }

        public void Set(float value)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            DevourRuntimeHelpers.ThrowIfNaN(value);
#endif
            float delta = value - _value;
            var v = MathModule.Clamp(value, _min, _max, out var rmin, out var rmax);
            float safeDelta = v - _value;
            SetValue(v, delta, safeDelta, rmin, rmax);
        }

        /// <summary>
        /// Изменил название вместо создания перегрузки 
        /// для явности. Думал назвать SetSilent, но это
        /// вводило бы в заблуждение
        /// </summary>
        /// <param name="value"></param>
        /// <param name=""></param>
        public void Set2(float value, bool raiseOnChange, bool raiseOnMin, bool raiseOnMax)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            DevourRuntimeHelpers.ThrowIfNaN(value);
#endif
            var v = MathModule.Clamp(value, _min, _max, out var rmin, out var rmax);

            if (raiseOnChange || raiseOnMin || raiseOnMax)
            {
                float ddelta = value - _value;
                float sdelta = v - _value;
                _value = v;

                if (raiseOnChange)
                    OnClampedValueChanged?.Invoke(this, ddelta, sdelta);

                if (raiseOnMin && rmin)
                    OnClampedValueReachedMin?.Invoke(this, ddelta, sdelta);

                if (raiseOnMax && rmax)
                    OnClampedValueReachedMax?.Invoke(this, ddelta, sdelta);
            }
            else
            {
                _value = v;
            }
        }

        public bool TryChange(float delta, bool inverse = false)
        {
            if (float.IsNaN(delta))
                return false;

            if (inverse)
                delta = -delta;

            Set(_value + delta);
            return true;
        }

        public bool TryAdd(float delta)
        {
            if (float.IsNegative(delta) || float.IsNaN(delta))
                return false;

            Add(delta);
            return true;
        }

        public bool TryRemove(float delta)
        {
            if (float.IsNegative(delta) || float.IsNaN(delta))
                return false;

            Remove(delta);
            return true;
        }

        public bool TrySet(float value)
        {
            if (float.IsNaN(value))
                return false;

            Set(value);
            return true;
        }
        #endregion

        #region clamped amount manipulatable
        public bool CanAddExact(float delta)
        {
            if (float.IsNegative(delta) || float.IsNaN(delta))
                return false;

            float desired = _value + delta;

            return desired <= _max && desired >= _min;
        }

        public bool CanRemoveExact(float delta)
        {
            if (float.IsNegative(delta) || float.IsNaN(delta))
                return false;

            float desired = _value - delta;

            return desired <= _max && desired >= _min;
        }

        public bool CanChangeExact(float delta)
        {
            if (delta > 0)
                return CanAddExact(delta);

            if (delta < 0)
                return CanRemoveExact(-delta);

            if (delta == 0)
                return true;

            return false;
        }

        public float AddSafe(float delta)
        {
            if (float.IsSubnormal(delta) || float.IsNegative(delta))
                return 0;

            float canAdd = _max - _value;

            if (delta < canAdd)
                canAdd = delta;

            Add(canAdd);
            return canAdd;
        }

        public float RemoveSafe(float delta)
        {
            if (float.IsSubnormal(delta) || float.IsNegative(delta))
                return 0;

            float canRemove = _min - _value;

            if (delta > canRemove)
                canRemove = delta;

            Remove(canRemove);
            return canRemove;
        }

        public float ChangeSafe(float delta, bool inverse = false)
        {
            if (float.IsSubnormal(delta) || delta == 0)
                return 0;

            if (inverse)
                delta = -delta;

            if (float.IsNegative(delta))
                return RemoveSafe(-delta);

            return AddSafe(delta);
        }

        public float SetSafe(float value)
        {
            var v = System.Math.Clamp(value, _min, _max);
            Set(v);
            return v;
        }
        #endregion

        protected void SetValue(float value, float dirtyDelta, float safeDelta, bool reachedMin, bool reachedMax)
        {
            _value = value;
            OnFloatValueChanged?.Invoke(this, safeDelta);
            OnClampedValueChanged?.Invoke(this, dirtyDelta, safeDelta);


            if (reachedMin)
                OnClampedValueReachedMin?.Invoke(this, dirtyDelta, safeDelta);
            else if (reachedMax)
                OnClampedValueReachedMax?.Invoke(this, dirtyDelta, safeDelta);
        }


        protected void SetMinInternal(float v)
        {
            _min = v;
        }

        protected void SetMaxInternal(float v)
        {
            _max = v;
        }

        protected void SetValueInternal(float v)
        {
            _value = v;
        }



    }
}
