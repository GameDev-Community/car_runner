using DevourDev.Unity.Utils.SimpleStats.Modifiers;
using System.Collections.Generic;

namespace DevourDev.Unity.Utils.SimpleStats
{
    public class StatsCollection
    {
        public event System.Action<StatsCollection, IStatData> OnStatObjectAdded;
        public event System.Action<StatsCollection, IStatData> OnStatObjectRemoved;
        public event System.Action<StatsCollection, IStatData, float, float> OnStatValueChanged;

        private readonly Dictionary<StatObject, IStatData> _stats;
        private readonly Dictionary<StatObject, CountingDictionary<StatModifier>> _unappliedModifiers;


        public StatsCollection()
        {
            _stats = new();
            _unappliedModifiers = new();
        }


        public bool TryAddStatObject(StatObject statObject, IStatData statData)
        {
            if (!_stats.TryAdd(statObject, statData))
                return false;


            statData.OnValueChanged += HandleStatValueChanged;

            if (_unappliedModifiers.TryGetValue(statObject, out var umcd))
            {
                foreach (KeyValuePair<StatModifier, RefInt> item in umcd)
                {

                }
            }

            OnStatObjectAdded?.Invoke(this, statData);
            return true;
        }


        public void AddModifier(Modifiers.StatModifierCreator mc)
        {
            AddModifier(mc.StatObject, mc.Create());
        }

        public void AddModifier(StatObject statObject, Modifiers.StatModifier m)
        {

        }

        private void HandleStatValueChanged(IStatData sender, float dirtyDelta, float safeDelta)
        {
            OnStatValueChanged?.Invoke(this, sender, dirtyDelta, safeDelta);
        }
    }
}