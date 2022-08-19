using DevourDev.Base.Collections.Generic;

namespace Externals.Utils.StatsSystem.Modifiers
{
    public class StatModifiersCollection
    {
        public event System.Action<StatModifiersCollection> OnModified;

        private readonly CountingDictionary<StatModifier> _modifiers;
        private readonly ModifiersOptimizer _optimizer;
        private readonly CountingDictionary<StatModifier> _addQueue;
        private readonly CountingDictionary<StatModifier> _removeQueue;


        public StatModifiersCollection()
        {
            _modifiers = new();
            _optimizer = new();
            _addQueue = new();
            _removeQueue = new();
        }


        public float ModifyValue(float source)
        {
            return _optimizer.ModifyValue(source);
        }


        public bool ContainsModifier(StatModifier m)
        {
            return _modifiers.Contains(m);
        }

        public bool TryGetModifierAmount(StatModifier m, out int amount)
        {
            return _modifiers.TryGetAmount(m, out amount);
        }


        public void AddModifier(StatModifier m, int amount)
        {
            _addQueue.AddAmount(m, amount);
        }


        public bool TryRemoveModifier(StatModifier m, int amount)
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (amount < 0)
                throw new System.Exception($"attempt to remove negative amount: {amount}");
#endif
            if (!_modifiers.TryGetAmount(m, out var amt))
                return false;

            if (amt < amount)
                return false;

            _modifiers.RemoveAmount(m, amount);

            m.Unmodify(_optimizer, amount);
            return true;
        }

        public void RemoveModifier(StatModifier m, int amount)
        {
            _removeQueue.AddAmount(m, amount);
        }


        public void FinishAddingModifiers()
        {
            ProcessAddQueue();
        }

        public void FinishRemovingModifiers()
        {
            ProcessRemoveQueue();
        }


        public (StatModifier, int)[] GetModifiers()
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (_addQueue.Count + _removeQueue.Count > 0)
            {
                throw new System.Exception($"забытая очередь. {_addQueue.Count}, {_removeQueue.Count}");
            }
#endif

            (StatModifier, int)[] res = new (StatModifier, int)[_modifiers.Count];

            int i = -1;
            foreach (var item in _modifiers)
            {
                res[++i] = (item.Key, item.Value);
            }

            return res;
        }

        public CountingDictionary<StatModifier> GetModifiersDictionary()
        {
#if DEVOUR_DEBUG || UNITY_EDITOR
            if (_addQueue.Count + _removeQueue.Count > 0)
            {
                throw new System.Exception($"забытая очередь. {_addQueue.Count}, {_removeQueue.Count}");
            }
#endif

            return _modifiers;
        }


        private void ProcessAddQueue()
        {
            foreach (var m in _addQueue)
            {
                var mdf = m.Key;
                var amt = m.Value;

                _modifiers.AddAmount(mdf, amt);
                mdf.Modify(_optimizer, amt);
            }

            _addQueue.Clear();
            RaiseModifiedEvent();
        }

        private void ProcessRemoveQueue()
        {
            foreach (var m in _removeQueue)
            {
                var mdfToRemove = m.Key;
                var amountToRemove = m.Value;

                _modifiers.RemoveAmountOrAny(mdfToRemove, amountToRemove);
                mdfToRemove.Unmodify(_optimizer, amountToRemove);
            }

            _removeQueue.Clear();
            RaiseModifiedEvent();
        }

        private void RaiseModifiedEvent()
        {
            OnModified?.Invoke(this);
        }
    }
}
