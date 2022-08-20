using Externals.Utils.StatsSystem;
using Game.Core.Car;
using UnityEngine;
using Utils.Attributes;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
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
        }
    }
}
