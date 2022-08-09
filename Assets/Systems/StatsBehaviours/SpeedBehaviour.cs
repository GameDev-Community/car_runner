using Game.Core;
using Game.Stats;
using UnityEngine;
using Utils;

namespace Game.StatsBehaviours
{
    public class SpeedBehaviour : MonoBehaviour
    {
        [SerializeField] private Racer _racer;
        [SerializeField] private CarController2 _controller;
        [SerializeField] private StatObject _maxSpeedStat;
        [SerializeField] private StatObject _currentSpeedStat;
        [SerializeField] private StatObject _accelerationStat;


        private ClampedFloat _maxSpeedData;
        private ClampedFloat _curSpeedData;
        private ClampedFloat _accData;


        private void Start()
        {
            var r = _racer;

            if (!r.TryGetStatData(_maxSpeedStat, out IClampedValue icv))
                throw new System.Collections.Generic.KeyNotFoundException(nameof(_maxSpeedStat));

            _maxSpeedData = (ClampedFloat)icv;

            if (!r.TryGetStatData(_currentSpeedStat, out icv))
                throw new System.Collections.Generic.KeyNotFoundException(nameof(_curSpeedData));

            _curSpeedData = (ClampedFloat)icv;

            if (!r.TryGetStatData(_accelerationStat, out icv))
                throw new System.Collections.Generic.KeyNotFoundException(nameof(_accelerationStat));

            _accData = (ClampedFloat)icv;

            Subscribe();
        }

        private void Subscribe()
        {
            _maxSpeedData.OnValueChanged += HandleMaxSpeedChanged;
            _curSpeedData.OnValueChanged += HandleCurrentSpeedChanged;
            _accData.OnValueChanged += HandleAccelerationChanged;

            HandleMaxSpeedChanged(_maxSpeedData, 0, 0);
            HandleCurrentSpeedChanged(_curSpeedData, 0, 0);
            HandleAccelerationChanged(_accData, 0, 0);
        }


        private void HandleMaxSpeedChanged(ClampedFloat se, float dd, float sd)
        {
            _controller.MaxSpeed = se.Value;
        }

        private void HandleCurrentSpeedChanged(ClampedFloat se, float dd, float sd)
        {
            _controller.CurSpeed = se.Value;
        }

        private void HandleAccelerationChanged(ClampedFloat se, float dd, float sd)
        {
            _controller.Acceleration = se.Value;
        }

    }
}
