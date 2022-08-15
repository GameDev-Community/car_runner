using DevourDev.Base.Numerics;
using DevourDev.Unity.Utils.SimpleStats.Modifiers;
using System.Collections;
using System.Collections.Generic;

namespace DevourDev.Base.Collections.Generic
{
    public sealed class CountingDictionary<T> : IEnumerable<KeyValuePair<T, RefInt>>, IEnumerable
    {
        private readonly Dictionary<T, RefInt> _items;
        private readonly bool _allowNegativeValues;


        public CountingDictionary(bool allowNegativeValues = false, int capacity = 0)
        {
            _items = new(capacity);
            _allowNegativeValues = allowNegativeValues;
        }


        public int Count => _items.Count;


        public bool Contains(T key)
            => _items.ContainsKey(key);

        public bool TryAddAmount(T key, int amount = 1)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (amount < 0)
                throw new System.ArgumentOutOfRangeException($"{nameof(amount)} is negative ({amount})");
#endif

            return TryChangeAmount(key, amount);
        }

        public bool TryRemoveAmount(T key, int amount = 1)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (amount < 0)
                throw new System.ArgumentOutOfRangeException($"{nameof(amount)} is negative ({amount})");
#endif

            return TryChangeAmount(key, -amount);
        }


        /// <returns>removed amount (-1 if key not found,
        /// 0 if allOrNothing and amount of item is lower
        /// than requested</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public int RemoveAmount(T key, int desiredRemoveAmount, bool allOrNothing)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (desiredRemoveAmount < 0)
                throw new System.ArgumentOutOfRangeException($"{nameof(desiredRemoveAmount)} is negative ({desiredRemoveAmount})");
#endif

            if (!TryGetAmountRef(key, out var refAmount))
                return -1;

            int newAmount = refAmount.Value - desiredRemoveAmount;

            if (_allowNegativeValues)
            {
                if (newAmount == 0)
                    _items.Remove(key);
            }
            else
            {
                if (allOrNothing && newAmount < 0)
                    return 0;

                if (newAmount <= 0)
                {
                    _items.Remove(key);
                    return refAmount.Value;
                }
            }

            refAmount.Value = newAmount;
            return desiredRemoveAmount;

        }

        public bool TryRemoveEntry(T key)
        {
            return _items.Remove(key);
        }




        public bool TryGetAmount(T key, out int amount)
        {
            if (_items.TryGetValue(key, out var refAmount))
            {
                amount = refAmount.Value;
                return true;
            }

            amount = 0;
            return false;
        }

        public bool TryGetAmountRef(T key, out RefInt amount)
        {
            return _items.TryGetValue(key, out amount);
        }

        public bool TryChangeAmount(T key, int delta)
        {
            if (delta == 0)
                return _items.ContainsKey(key);

            if (!_items.TryGetValue(key, out var amount))
            {
                if (_allowNegativeValues || delta > 0)
                {
                    _items.Add(key, new(delta));
                    return true;
                }

                return false;
            }

            int newAmount = amount + delta;

            if (newAmount == 0)
            {
                _items.Remove(key);
            }
            else if (_allowNegativeValues || newAmount > 0)
            {
                amount.Set(newAmount);
            }
            else
            {
                return false;
            }

            return true;
        }


        IEnumerator<KeyValuePair<T, RefInt>> IEnumerable<KeyValuePair<T, RefInt>>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }


        public void Clear()
        {
            _items.Clear();
        }

        //public bool TryRemove(System.Predicate<StatModifier> predicate, int amount, bool allOrNothing)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}

