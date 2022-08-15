using UnityEngine;

namespace Game.Car
{
    public class CarDecorator : MonoBehaviour
    {
        [SerializeField] private CarController _controller;
        [SerializeField] private CarDashboardUi _dashboard;

        [Space]
        [SerializeField] private Transform[] _wheelTransforms;


        private void Start()
        {
            var c = _controller;
            c.OnMetricsUpdated += HandleMetricsUpdated;
            c.OnWheelColliderGroundedStateChanged += HandleWheelColliderGroundedStateChanged;
            c.OnWheelColliderWorldPosChanged += HandleWheelColliderWorldPosChanged;
            c.OnWheelColliderWorldRotChanged += HandleWheelColliderWorldRotChanged;
        }


        private void HandleMetricsUpdated(CarController arg1, CarController.CarMetrics metrics)
        {
            _dashboard.SetAvgRpm(metrics.DrivingWheelsAvgRpm);
        }

        private void HandleWheelColliderGroundedStateChanged(CarController arg1, int index, bool state)
        {
            UnityEngine.Debug.Log($"wheel num {index} {(state ? "ебанулось" : "съебалось")}");
        }

        private void HandleWheelColliderWorldPosChanged(CarController arg1, int index, Vector3 worldPos)
        {
            _wheelTransforms[index].position = worldPos;
        }

        private void HandleWheelColliderWorldRotChanged(CarController arg1, int index, Quaternion worldRot)
        {
            _wheelTransforms[index].rotation = worldRot;
        }

    }
}