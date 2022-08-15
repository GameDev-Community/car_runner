using DevourDev.Unity.Utils.SimpleStats;
using DevourDev.Unity.Utils.SimpleStats.Modifiers;
using UnityEngine;

namespace Game.Core
{
    public class StatsHolder : MonoBehaviour, IStatsHolder
    {
        [System.Serializable]
        private struct StatInitor
        {
            public StatObject StatObject;
            public float SourceValue;
        }

        [SerializeField, NonReorderable] private StatInitor[] _initialStats;
        [SerializeField, NonReorderable] private StatModifierCreator[] _initialModifiers;

        private StatsCollection _stats;

        public StatsCollection Stats => _stats;
    }
}
