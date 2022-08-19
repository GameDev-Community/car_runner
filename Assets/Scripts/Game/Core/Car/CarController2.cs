using UnityEngine;
using Utils;

namespace Game.Core.Car
{
    public class CarController2 : MonoBehaviour, ICarController
    {
        [SerializeField] private float _sidewaysSpeed = 400f;
        [SerializeField] private float _roadHalfWidth = 4f;

        [SerializeField] private Speedometer _speedometer;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Vector3 _forceDir = Vector3.forward;


        private bool _grounded;
        private float _verticalMovingV;
        private float _horizontalMovingV;


        public float MaxSpeed { get; set; } = 15f;
        public float Acceleration { get; set; } = 100f;

        public float Speed
        {
            get => _rb.velocity.z;

            set
            {
                var v = _rb.velocity;
                v.z = value;
                _rb.velocity = v;
            }
        }

        public bool Grounded => _grounded;
        public float VerticalMoving => _verticalMovingV;
        public float HorizontalMoving => _horizontalMovingV;


        public void SetHorizontalMoving(float v)
        {
            float desiredDelta = v * _sidewaysSpeed * Time.fixedDeltaTime;
            float safeDelta;

            //desiredDelta = -1

            if (v > 0)
            {
                safeDelta = _roadHalfWidth - _rb.position.x;
                if (desiredDelta < safeDelta)
                    safeDelta = desiredDelta;
            }
            else
            {
                safeDelta = -_rb.position.x - _roadHalfWidth;
                if (desiredDelta > safeDelta)
                    safeDelta = desiredDelta;
            }


            //Debug.Log(safeDelta);
            var p = _rb.position;
            p.x += safeDelta;
            _rb.position = p;
        }


        public void SetVerticalMoving(float v)
        {
            throw new System.NotSupportedException("в этом гиперкозле нет такой возможности");
        }


        private void FixedUpdate()
        {
            SetHorizontalMoving(Input.GetAxis("Horizontal"));

            _grounded = Physics.Raycast(_rb.position, transform.up * -1f, 2f);

            var rb = _rb;
            if (_grounded)
            {
                var acc = Acceleration;

                if (acc < 0)
                    acc = 0;

                rb.AddForce(acc * _forceDir, ForceMode.Acceleration);
                var v = _rb.velocity;

                if (System.Math.Abs(v.z) > MaxSpeed)
                {
                    v.z = MaxSpeed * System.Math.Sign(v.z);
                    rb.velocity = v;
                }

                //_speedometer.SetSpeed(v.z);
                //_speedometer.SetAcceleration(acc);
            }
            else
            {
                float angle = transform.rotation.eulerAngles.x;

                if (angle > 180)
                    angle -= 360;

                angle = -angle;

                Vector3 currentAngularVelocity = rb.angularVelocity;
                currentAngularVelocity.x = 1.2f * angle * Time.fixedDeltaTime;
                rb.angularVelocity = currentAngularVelocity;
            }

        }
    }
}
