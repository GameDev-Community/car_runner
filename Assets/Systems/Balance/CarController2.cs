using UnityEngine;

namespace Game.Core
{
    public class CarController2 : MonoBehaviour
    {
        [SerializeField] private float _sidewaysSpeed = 400f;
        [SerializeField] private float _roadHalfWidth = 4f;

        [SerializeField] private Speedometer _speedometer;
        [SerializeField] private Stats.StatObject _accelerationStat;
        [SerializeField] private Racer _racer;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Vector3 _forceDir = Vector3.forward;


        private bool _grounded;


        public float MaxSpeed { get; set; }
        public float Acceleration { get; set; }

        public float CurSpeed
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


        public void MoveSideways(float v)
        {
            float desiredDelta = v * _sidewaysSpeed * Time.fixedDeltaTime;
            float safeDelta;

            //desiredDelta = -1

            if(v > 0)
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


            Debug.Log(safeDelta);
            var p = _rb.position;
            p.x += safeDelta;
            _rb.position = p;
        }

        private void FixedUpdate()
        {
            MoveSideways(Input.GetAxis("Horizontal"));

            _grounded = Physics.Raycast(_rb.position, transform.up * -1f, 0.5f);

            var rb = _rb;
            if (_grounded)
            {
                var acc = _racer.ProcessStatValue(_accelerationStat, Acceleration);

                if (acc < 0)
                    acc = 0;

                rb.AddForce(_forceDir * acc * Time.fixedDeltaTime, ForceMode.Acceleration);
                var v = _rb.velocity;

                if (System.Math.Abs(v.z) > MaxSpeed)
                {
                    v.z = MaxSpeed * System.Math.Sign(v.z);
                    rb.velocity = v;
                }

                _speedometer.SetSpeed(v.z);
                _speedometer.SetAcceleration(acc);
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
