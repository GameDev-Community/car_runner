using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Car
{
    public class CarDecorator : MonoBehaviour
    {
        [SerializeField] private CarController _controller;
        [SerializeField] private CarDashboardUi _dashboard;

        [Space]
        [SerializeField] private Transform carMeshTransform;
        [SerializeField] private Transform[] _wheelTransforms;
        [SerializeField] private Transform[] _steeringWheelTransforms;

        [SerializeField] private float wheelSteerAngle = 35f;
        [SerializeField] private float carAngle = 5f;

        private void Start()
        {
            _controller.OnWheelColliderWorldRotChanged += HandleWheelColliderWorldRotChanged;
            _controller.OnWheelColliderWorldPosChanged += HandleWheelColliderWorldPosChanged;
            _controller.OnTurnChanged += HandleTurnChanged;
        }

        private void HandleWheelColliderWorldRotChanged(CarController arg1, int index, Quaternion worldRot)
        {
            _wheelTransforms[index].rotation = worldRot;
        }

        private void HandleWheelColliderWorldPosChanged(CarController arg1, int index, Vector3 worldPos)
        {
            _wheelTransforms[index].position = worldPos;
        }

        private void HandleTurnChanged(CarController arg1, float turnDirection)
        {
            for (int i = 0; i < _steeringWheelTransforms.Length; i++)
            {
                Quaternion currentQuaternion = _steeringWheelTransforms[i].localRotation;
                Quaternion steeringQuaternion = Quaternion.Euler(Vector3.up * wheelSteerAngle * turnDirection);
                _steeringWheelTransforms[i].localRotation = steeringQuaternion * currentQuaternion;
            }

            carMeshTransform.localRotation = Quaternion.Euler(Vector3.up * carAngle * turnDirection);
        }
    }
}
