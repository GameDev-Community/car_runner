using System.Collections.Generic;
using UnityEngine;

namespace Game.Car
{

    public class CarController : MonoBehaviour
    {
        private class WheelInfo
        {
            public WheelCollider WheelCollider;
            public int WheelIndex;
            public float LastMotorTorque;
            public float LastBrakeTorque;
            public Vector3 LastWorldPos;
            public Quaternion LastWorldRot;
            public bool LastGroundedState;


            public WheelInfo(WheelCollider wc, int index)
            {
                WheelCollider = wc;
                WheelIndex = index;
                LastMotorTorque = default;
                LastBrakeTorque = default;
                LastWorldPos = default;
                LastWorldRot = default;
                LastGroundedState = default;
            }
        }


        public struct CarMetrics
        {
            public float DrivingWheelsAvgRpm;
        }


        /// <summary>
        /// sender, wheel index, world position
        /// </summary>
        public event System.Action<CarController, int, Vector3> OnWheelColliderWorldPosChanged;
        /// <summary>
        /// sender, wheel index, world rotation
        /// </summary>
        public event System.Action<CarController, int, Quaternion> OnWheelColliderWorldRotChanged;
        /// <summary>
        /// sender, wheel index, new grounded state
        /// </summary>
        public event System.Action<CarController, int, bool> OnWheelColliderGroundedStateChanged;
        /// <summary>
        /// sender, speed (for speedometer)
        /// </summary>
        public event System.Action<CarController, CarMetrics> OnMetricsUpdated;


        [SerializeField] private Rigidbody _mainRigidbody;

        [Space]
        [Header("Wheels' Colliders")]
        [SerializeField] private WheelCollider[] _allWheelColliders;

        [Space]
        [SerializeField] private float _motorForce;
        [SerializeField] private float _brakingForce;

        [SerializeField] private float _maxSteeringAngle;

        [Tooltip("Привод")]
        [SerializeField] private WheelCollider[] _drivingUnits;
        [Tooltip("Колёса, которые поворачиваются")]
        [SerializeField] private WheelCollider[] _steeringUnits;

        private float _horizontalV;
        private float _verticalV;
        private float _verticalMultiplier;
        private float _braking;

        private WheelInfo[] _allWheelsInfo;
        private Dictionary<WheelCollider, int> _indexesByCollider;
        private WheelInfo[] _drivingWheels;
        private WheelInfo[] _steeringWheels;

        private CarMetrics _lastMetrics;


        public float HorizontalV => _horizontalV;
        public float VerticalV => _verticalV;
        public CarMetrics Metrics => _lastMetrics;


        private void Awake()
        {
            var aw = _allWheelColliders;
            var awc = aw.Length;
            var awi = new WheelInfo[awc];
            var awdic = new Dictionary<WheelCollider, int>(awc);

            for (int i = -1; ++i < awc;)
            {
                var wc = aw[i];
                awi[i] = new WheelInfo(wc, i);
                awdic.Add(wc, i);
            }

            var du = _drivingUnits;
            var duc = du.Length;
            var dw = new WheelInfo[duc];

            for (int i = -1; ++i < duc;)
                dw[i] = awi[awdic[du[i]]];

            var su = _steeringUnits;
            var suc = su.Length;
            var sw = new WheelInfo[suc];

            for (int i = -1; ++i < suc;)
                sw[i] = awi[awdic[su[i]]];

            _allWheelsInfo = awi;
            _indexesByCollider = awdic;
            _drivingWheels = dw;
            _steeringWheels = sw;
        }



        private void Update()
        {
            SetHorizontalMoveValue(Input.GetAxis("Horizontal"));
            SetVerticalMoveValue(Input.GetAxis("Vertical"));
            SetBrakingValue(Input.GetKey(KeyCode.Space) ? 1 : 0);
            SetVericalMultiplierValue(Input.GetKey(KeyCode.LeftShift) ? 10 : 0);

        }


        private void FixedUpdate()
        {
            HandleMotor();
            HandleSteering();
            CheckForChanges();
        }


        public WheelCollider GetWheelColliderByIndex(int index)
        {
            return _allWheelColliders[index];
        }


        /// <param name="horizontal">[-1, 1]</param>
        public void SetHorizontalMoveValue(float horizontal)
        {
            //to prevent redunant events raising
            if (_horizontalV == horizontal)
                return;

            _horizontalV = horizontal;
        }

        /// <param name="vertical">[-1, 1]</param>
        public void SetVerticalMoveValue(float vertical)
        {
            //to prevent redunant events raising
            if (_verticalV == vertical)
                return;

            _verticalV = vertical;
        }

        public void SetVericalMultiplierValue(float verticalMultiplier)
        {
            if (_verticalMultiplier == verticalMultiplier)
                return;

            _verticalMultiplier = verticalMultiplier;
        }

        /// <param name="braking">[0, 1]</param>
        public void SetBrakingValue(float braking)
        {
            //to prevent redunant events raising
            if (_braking == braking)
                return;

            _braking = braking;
        }


        private void HandleMotor()
        {
            var arr = _drivingWheels;
            var c = arr.Length;

            var torque = _verticalV * (1 + _verticalMultiplier) * _motorForce;

            for (int i = -1; ++i < c;)
                SetWheelTorque(arr[i], torque);


            var brakeTorque = _braking * _brakingForce;

            for (int i = -1; ++i < c;)
                SetWheelBrakingTorque(arr[i], brakeTorque);


        }

        private void HandleSteering()
        {
            if (_horizontalV == 0)
                return;

            var angle = _maxSteeringAngle * _horizontalV;

            var arr = _steeringWheels;
            var c = arr.Length;

            for (int i = -1; ++i < c;)
                SetWheelSteeringAngle(arr[i], angle);
        }

        private void CheckForChanges()
        {
            var colsArr = _allWheelColliders;
            var infosArr = _allWheelsInfo;
            var c = infosArr.Length;

            for (int i = -1; ++i < c;)
            {
                var info = infosArr[i];
                var col = colsArr[i];
                col.GetWorldPose(out var pos, out var rot);

                if (info.LastWorldPos != pos)
                {
                    info.LastWorldPos = pos;
                    OnWheelColliderWorldPosChanged?.Invoke(this, i, pos);
                }

                if (info.LastWorldRot != rot)
                {
                    info.LastWorldRot = rot;
                    OnWheelColliderWorldRotChanged?.Invoke(this, i, rot);
                }

                var gstate = col.isGrounded;
                if (info.LastGroundedState != gstate)
                {
                    info.LastGroundedState = gstate;
                    OnWheelColliderGroundedStateChanged?.Invoke(this, i, gstate);
                }
            }

            var tmpMetrics = _lastMetrics;

            float avgRpm = 0;
            var duArr = _drivingUnits;
            var duArrC = duArr.Length;

            for (int i = -1; ++i < duArrC; i++)
            {
                WheelCollider item = duArr[i];
                avgRpm += item.rpm;
            }

            avgRpm /= duArrC;


            if (tmpMetrics.DrivingWheelsAvgRpm != avgRpm)
            {
                var newMetrics = new CarMetrics
                {
                    DrivingWheelsAvgRpm = avgRpm,
                };

                _lastMetrics = newMetrics;
                OnMetricsUpdated?.Invoke(this, newMetrics);
            }
        }


        private void SetWheelTorque(WheelInfo info, float v)
        {
            if (info.LastMotorTorque == v)
                return;

            info.WheelCollider.motorTorque = info.LastMotorTorque = v;

            //events
        }

        private void SetWheelBrakingTorque(WheelInfo info, float v)
        {
            if (info.LastBrakeTorque == v)
                return;

            info.WheelCollider.brakeTorque = info.LastBrakeTorque = v;
        }

        private void SetWheelSteeringAngle(WheelInfo wheelInfo, float angle)
        {
            //check
            wheelInfo.WheelCollider.steerAngle = angle;
        }
    }
}