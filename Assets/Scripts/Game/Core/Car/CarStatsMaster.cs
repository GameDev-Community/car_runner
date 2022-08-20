using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;
using UnityEngine;
using Utils.Attributes;

namespace Game.Core.Car
{
    /// <summary>
    /// класс создаёт, добавляет и пользуется статами, связанными с машиной - настоящий дунген мастер
    /// </summary>
    public class CarStatsMaster : StatsBehaviour
    {
        [SerializeField, RequireInterface(typeof(ICarController)), InspectorName("Car Controller")] UnityEngine.Object _carController_raw;

        [SerializeField] private FloatDynamicStatDataCreator _speedStatDataCreator;
        [SerializeField] private FloatModifiableStatDataCreator _accelerationStatDataCreator;

        private ICarController _carController;


        protected override void Awake()
        {
            base.Awake();
            _carController = (ICarController)_carController_raw;

            var speedStatData = _speedStatDataCreator.Create();
            speedStatData.OnBoundsChanged += MaxSpeed_OnBoundsChanged;
            speedStatData.OnClampedValueChanged += Speed_OnChange;
            StatsHolder.StatsCollection.AddStat(_speedStatDataCreator.StatObject, speedStatData);


            var accelerationStatData = _accelerationStatDataCreator.Create();
            accelerationStatData.OnFloatValueChanged += Acceleration_OnChanged;
            StatsHolder.StatsCollection.AddStat(_accelerationStatDataCreator.StatObject, accelerationStatData);

            MaxSpeed_OnBoundsChanged(speedStatData, Vector3.zero);
            Speed_OnChange(speedStatData, 0, 0);
            Acceleration_OnChanged(accelerationStatData, 0);
        }


        private void MaxSpeed_OnBoundsChanged(ClampedFloat sender, Vector3 prev)
        {
            _carController.MaxSpeed = sender.Max;
        }

        private void Speed_OnChange(ClampedFloat sender, float dirty, float safe)
        {
            _carController.Speed = sender.Value;
        }

        private void Acceleration_OnChanged(IFloatValueCallback sender, float delta)
        {
            _carController.Acceleration = sender.Value;
        }
    }
}
