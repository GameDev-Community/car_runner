using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;
using Game.Core.Car;
using UnityEngine;
using Utils.Attributes;

namespace Game.Stats
{
    /// <summary>
    /// класс создаёт, добавляет и пользуется статами, связанными с машиной - настоящий дунген мастер
    /// </summary>
    public class CarStatsBehaviour : StatsBehaviour
    {
        [SerializeField, RequireInterface(typeof(ICarController)), InspectorName("Car Controller")] UnityEngine.MonoBehaviour _carController_raw;

        [SerializeField] private FloatDynamicStatDataCreator _speedStatDataCreator;
        [SerializeField] private FloatModifiableStatDataCreator _accelerationStatDataCreator;

        private ICarController _carController;
        private ClampedFloat _speedCF;


        protected override void Awake()
        {
            base.Awake();
            _carController = (ICarController)_carController_raw;

            var speedStatData = _speedStatDataCreator.Create();
            _speedCF = speedStatData;
            speedStatData.OnBoundsChanged += MaxSpeed_OnBoundsChanged;
            speedStatData.OnClampedValueChanged += Speed_OnChange;
            StatsHolder.StatsCollection.AddStat(_speedStatDataCreator.StatObject, speedStatData);


            var accelerationStatData = _accelerationStatDataCreator.Create();
            accelerationStatData.OnValueChanged += Acceleration_OnChanged;
            StatsHolder.StatsCollection.AddStat(_accelerationStatDataCreator.StatObject, accelerationStatData);

            MaxSpeed_OnBoundsChanged(speedStatData, Vector3.zero);
            Speed_OnChange(speedStatData, 0, 0);
            Acceleration_OnChanged(accelerationStatData, 0);
        }


        private void FixedUpdate()
        {
            // mb move to CarController
            _speedCF.Set2(_carController.Speed, false, false, false);
        }

        private void MaxSpeed_OnBoundsChanged(ClampedFloat sender, Vector3 prev)
        {
            UnityEngine.Debug.Log($"MaxSpeed Set: {sender.Max}");
            _carController.MaxSpeed = sender.Max;
        }

        private void Speed_OnChange(ClampedFloat sender, float dirty, float safe)
        {
            _carController.Speed = sender.Value;
        }

        private void Acceleration_OnChanged(IValueCallback<float> sender, float delta)
        {
            _carController.Acceleration = sender.Value;
        }
    }
}
