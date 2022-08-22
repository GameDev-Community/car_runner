using Externals.Utils.Valuables;
using System;

namespace Externals.Utils.StatsSystem
{
    public class IntStatData : IStatData, IIntValueCallback, IAmountManipulatable<int>
    {
        /// <summary>
        /// sender, delta
        /// </summary>
        public event System.Action<IIntValueCallback, int> OnIntValueChanged;

        private readonly StatObject _statObject;
        private int _value;

        public IntStatData(StatObject statObject, int initialValue)
        {
            _statObject = statObject;
            _value = initialValue;
        }


        public StatObject StatObject => _statObject;
        public int Value => _value;


        #region IOperatiable

        public void Change(int delta, bool inverse = false)
        {
            if (inverse)
                delta = -delta;

            if (delta > 0)
            {
                Add(delta);
            }
            else if (delta < 0)
            {
                Remove(-delta);
            }
        }

        public bool CanChange(int delta, out int result)
        {
            if (delta > 0)
            {
                return CanAdd(delta, out result);
            }
            else if (delta < 0)
            {
                return CanRemove(-delta, out result);
            }

            result = _value;
            return true;
        }

        public bool TryChange(int delta, bool inverse = false)
        {
            if (inverse)
                delta = -delta;

            if (CanChange(delta, out var result))
            {
                _value = result;
                OnIntValueChanged?.Invoke(this, delta);
                return true;
            }

            return false;
        }

        public void Add(int delta)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (delta < 0)
                throw new Exception("value should be positive");
#endif
            _value += delta;
        }

        public bool CanAdd(int delta, out int result)
        {
            if (delta < 0)
                goto Fail;

            long checkRes = (long)_value + (long)delta;

            if (checkRes <= int.MaxValue)
            {
                result = (int)checkRes;
                return true;
            }

Fail:
            result = default;
            return false;
        }

        public bool CanRemove(int delta, out int result)
        {
            if (delta < 0)
                goto Fail;

            long checkRes = (long)_value - (long)delta;

            if (checkRes >= int.MinValue)
            {
                result = (int)checkRes;
                return true;
            }

Fail:
            result = default;
            return false;
        }

        public bool CanSet(int value)
        {
            return true;
        }

        public void Remove(int delta)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (delta < 0)
                throw new Exception("value should be positive");
#endif
            _value -= delta;
            OnIntValueChanged?.Invoke(this, delta);
        }

        public void Set(int value)
        {
            int delta = value - _value;
            _value = value;
            OnIntValueChanged?.Invoke(this, delta);
        }

        public bool TryAdd(int delta)
        {
            if (CanAdd(delta, out var r))
            {
                _value = r;
                OnIntValueChanged?.Invoke(this, delta);
                return true;
            }

            return false;
        }

        public bool TryRemove(int delta)
        {
            if (CanRemove(delta, out var r))
            {
                _value = r;
                OnIntValueChanged?.Invoke(this, delta);
                return true;
            }

            return false;
        }

        public bool TrySet(int value)
        {
            if (CanSet(value))
            {
                int delta = value - _value;
                _value = value;
                OnIntValueChanged?.Invoke(this, delta);
                return true;
            }

            return false;
        }
        #endregion
    }
}
