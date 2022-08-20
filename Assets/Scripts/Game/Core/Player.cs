using Externals.Utils.StatsSystem;
using Game.Core.Car;
using UnityEngine;
using Utils.Attributes;

namespace Game.Core
{
    public class StatDataStateVisualizer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _statNameText;
        [SerializeField] private TMPro.TextMeshProUGUI _statStateText;


        public void Init(IStatData isd)
        {
            _statNameText.text = isd.StatObject.MetaInfo.Name;

            switch (isd)
            {
                case ClampedFloatStatData clampedFloatStatData:
                    clampedFloatStatData.OnBoundsChanged += ClampedFloatStatData_OnBoundsChanged;
                    break;
                default:
                    break;
            }
        }

        private void ClampedFloatStatData_OnBoundsChanged(ClampedFloat arg1, Vector3 arg2)
        {
            _statStateText.text = $"min: {arg1.Min}, max: {arg1.Max}, cur: {arg1.Value}";
        }
    }
    public class Player : MonoBehaviour
    {
        //debug

        [SerializeField] private StatDataStateVisualizer _statDataStateVisualizerPrefab;
        [SerializeField] private Transform _pizdaParent;

        [SerializeField, RequireInterface(typeof(ICarController)), InspectorName("Car Controller")] Object _carController_raw;
        [SerializeField, RequireInterface(typeof(IStatsHolder)), InspectorName("Stats Holder")] Object _statsHolder_raw;

        private ICarController _carController;
        private IStatsHolder _statsHolder;


        public ICarController CarController => _carController;
        public IStatsHolder StatsHolder => _statsHolder;


        private void Awake()
        {
            _carController = (ICarController)_carController_raw;
            _statsHolder = (IStatsHolder)_statsHolder_raw;

            //debug
            var stats = _statsHolder.StatsCollection;

            foreach (System.Collections.Generic.KeyValuePair<StatObject, IStatData> item in stats)
            {
                var xdd = Instantiate(_statDataStateVisualizerPrefab, _pizdaParent);
                xdd.Init(item.Value);
            }
        }

        private void FloatCallback_OnFloatValueChanged(IFloatValueCallback arg1, float arg2)
        {
            throw new System.NotImplementedException();
        }
    }
}
