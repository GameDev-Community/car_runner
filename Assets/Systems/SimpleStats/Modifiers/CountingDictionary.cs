using System.Collections;
using System.Collections.Generic;

namespace DevourDev.Unity.Utils.SimpleStats.Modifiers
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


        public bool TryAdd(T key, int amount = 1)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (amount < 0)
                throw new System.ArgumentOutOfRangeException($"{nameof(amount)} is negative ({amount})");
#endif

            return TryChangeAmount(key, amount);
        }

        public bool TryRemove(T key, int amount = 1)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (amount < 0)
                throw new System.ArgumentOutOfRangeException($"{nameof(amount)} is negative ({amount})");
#endif

            return TryChangeAmount(key, -amount);
        }


        public bool TryGetAmount(T key, out RefInt amount)
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

            int newAmount = delta + amount;

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
    }
}

