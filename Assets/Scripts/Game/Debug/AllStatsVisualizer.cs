using Externals.Utils.StatsSystem;
using System.Collections.Generic;
using UnityEngine;
using Utils.Attributes;

namespace Game.Debug
{
    public class AllStatsVisualizer : MonoBehaviour
    {
        [SerializeField] private StatDataStateVisualizer _statDataStateVisualizerPrefab;
        [SerializeField] private Transform _parent;
        [SerializeField, RequireInterface(typeof(IStatsHolder)), InspectorName("Stats Holder")] Object _statsHolder_raw;


        private Dictionary<StatObject, StatDataStateVisualizer> _slots;


        private void Awake()
        {
            _slots = new();
            var stats = ((IStatsHolder)_statsHolder_raw).StatsCollection;

            foreach (var item in stats)
            {
                var vz = Instantiate(_statDataStateVisualizerPrefab, _parent);
                _slots.Add(item.Key, vz);
                vz.Init(item.Value);
            }

            stats.OnStatAdded += Stats_OnStatAdded;
            stats.OnStatRemoved += Stats_OnStatRemoved;
        }

        private void Stats_OnStatRemoved(StatsCollection arg1, IStatData arg2)
        {
            if (!_slots.Remove(arg2.StatObject, out var vz))
            {
                throw new System.Exception($"{arg1}, {arg2}");
            }

            Destroy(vz.gameObject);
        }

        private void Stats_OnStatAdded(StatsCollection arg1, IStatData arg2)
        {
            var vz = Instantiate(_statDataStateVisualizerPrefab, _parent);
            _slots.Add(arg2.StatObject, vz);
            vz.Init(arg2);
        }
    }
}
