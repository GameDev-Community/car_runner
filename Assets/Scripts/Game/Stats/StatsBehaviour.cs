using Externals.Utils.StatsSystem;
using UnityEngine;
using Utils.Attributes;

namespace Game.Core.Car
{
    public abstract class StatsBehaviour : MonoBehaviour
    {
        [SerializeField, RequireInterface(typeof(IStatsHolder)), InspectorName("Stats Holder")] UnityEngine.Object _statsHolder_raw;

        private IStatsHolder _statsHolder;

        protected IStatsHolder StatsHolder => _statsHolder;


        protected virtual void Awake()
        {
            _statsHolder = (IStatsHolder)_statsHolder_raw;
        }
    }
}
