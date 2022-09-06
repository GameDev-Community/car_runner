using Externals.Utils.SaveManager;
using Externals.Utils.StatsSystem;
using Game.Core.Car;
using Game.Garage;
using Game.Helpers;
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

        private GarageData _garageData;


        public ICarController CarController => _carController;
        public IStatsHolder StatsHolder => _statsHolder;
        public GarageData GarageData => _garageData;


        private void Awake()
        {
            _garageData = new();

            try
            {
                _carController = (ICarController)_carController_raw;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }

            try
            {
                _statsHolder = (IStatsHolder)_statsHolder_raw;
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(ex);
            }

            
            SaveManager.OnSave += SaveManager_OnSave;
            SaveManager.OnLoad += SaveManager_OnLoad;
        }

        private void SaveManager_OnSave(System.IO.BinaryWriter bw)
        {
            //_statsHolder
        }

        private void SaveManager_OnLoad(System.IO.BinaryReader br)
        {
            throw new System.NotImplementedException();
        }

        

        public void Kill()
        {
            UnityEngine.Debug.Log("We are dead, what a surprise");
            _onDeath?.Invoke();
        }
    }
}
