using Externals.Utils.StatsSystem;
using Game.Core.Car;
using UnityEngine;
using UnityEngine.Events;
using Utils.Attributes;

namespace Game.Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField, RequireInterface(typeof(ICarController)), InspectorName("Car Controller")] Object _carController_raw;
        [SerializeField, RequireInterface(typeof(IStatsHolder)), InspectorName("Stats Holder")] Object _statsHolder_raw;

        [SerializeField] private UnityEvent _onDeath;

        private ICarController _carController;
        private IStatsHolder _statsHolder;


        public ICarController CarController => _carController;
        public IStatsHolder StatsHolder => _statsHolder;


        private void Awake()
        {
            _carController = (ICarController)_carController_raw;
            _statsHolder = (IStatsHolder)_statsHolder_raw;
        }


        public void Kill()
        {
            UnityEngine.Debug.Log("We are dead, what a surprise");
            _onDeath?.Invoke();
        }
    }
}
