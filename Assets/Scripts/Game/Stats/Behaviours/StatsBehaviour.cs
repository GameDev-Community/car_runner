using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;
using Game.Helpers;
using UnityEngine;
using Utils.Attributes;

namespace Game.Stats
{
    public abstract class StatsBehaviour : MonoBehaviour
    {
        [SerializeField, RequireInterface(typeof(IStatsHolder)), InspectorName("Stats Holder")] UnityEngine.MonoBehaviour _statsHolder_raw;

        private IStatsHolder _statsHolder;

        protected IStatsHolder StatsHolder => _statsHolder;


        protected virtual void Awake()
        {
            _statsHolder = (IStatsHolder)_statsHolder_raw;
        }
    }
}
