namespace DevourDev.Unity.Utils.SimpleStats.Modifiers
{
    public class StatModifiersCollection
    {
        public event System.Action<StatModifiersCollection> OnModified;

        private readonly CountingDictionary<StatModifier> _modifiers;
        private readonly ModifierOptimizer _optimizer;
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

        public void AddModifier(StatModifier m, int amount, bool recalculate)
        {
            _addQueue.TryAdd(m, amount);

            if (recalculate)
                ProcessAddQueue();
        }

        public void RemoveModifier(StatModifier m, int amount, bool recalculate)
        {
            _removeQueue.TryAdd(m, amount);

            if (recalculate)
                ProcessRemoveQueue();
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

                if (!_modifiers.TryAdd(mdf, amt))
                    throw new System.Exception($"unexpected behaviour: unable to change amount. {mdf}, {amt}");

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

                if (!_modifiers.TryRemove(mdfToRemove, amountToRemove))
                {
                    throw new System.ArgumentOutOfRangeException(nameof(amountToRemove) + ": " + amountToRemove);
                }

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

