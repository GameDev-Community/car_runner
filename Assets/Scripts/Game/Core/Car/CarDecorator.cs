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
        [SerializeField] private Transform _carMeshTransform;
        [SerializeField] private Transform[] _wheelTransforms;
        [SerializeField] private int[] _steeringWheelIndexes;

        [SerializeField] private float _wheelSteerAngle = 35f;
        [SerializeField] private float _carAngle = 5f;


        public Transform[] WheelTransforms { get => _wheelTransforms; set => _wheelTransforms = value; }


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
            for (int i = 0; i < _steeringWheelIndexes.Length; i++)
            {
                var wheelTransform = _wheelTransforms[_steeringWheelIndexes[i]];
                Quaternion currentQuaternion = wheelTransform.localRotation;
                Quaternion steeringQuaternion = Quaternion.Euler(_wheelSteerAngle * turnDirection * Vector3.up);
                wheelTransform.localRotation = steeringQuaternion * currentQuaternion;
            }

            _carMeshTransform.localRotation = Quaternion.Euler(_carAngle * turnDirection * Vector3.up);
        }
    }
}
