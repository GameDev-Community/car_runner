using DevourDev.Unity.Utils.SimpleStats;
using Game.Core;
using UnityEngine;

namespace Game.StatsBehaviours
{
    public class SpeedBehaviour : StatBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private StatObject _maxSpeedStat;
        [SerializeField] private StatObject _currentSpeedStat;
        [SerializeField] private StatObject _accelerationStat;


        //private ClampedFloat _maxSpeedData;
        //private ClampedFloat _curSpeedData;
        //private ClampedFloat _accData;


        private void Start()
        {
            //var r = _player;

            //if (!r.TryGetStatData(_maxSpeedStat, out IClampedValue icv))
            //    throw new System.Collections.Generic.KeyNotFoundException(nameof(_maxSpeedStat));

            //_maxSpeedData = (ClampedFloat)icv;

            //if (!r.TryGetStatData(_currentSpeedStat, out icv))
            //    throw new System.Collections.Generic.KeyNotFoundException(nameof(_curSpeedData));

            //_curSpeedData = (ClampedFloat)icv;

            //if (!r.TryGetStatData(_accelerationStat, out icv))
            //    throw new System.Collections.Generic.KeyNotFoundException(nameof(_accelerationStat));

            //_accData = (ClampedFloat)icv;

            //Subscribe();
        }

        private void Subscribe()
        {
            //_maxSpeedData.OnValueChanged += HandleMaxSpeedChanged;
            //_curSpeedData.OnValueChanged += HandleCurrentSpeedChanged;
            //_accData.OnValueChanged += HandleAccelerationChanged;

            //HandleMaxSpeedChanged(_maxSpeedData, 0, 0);
            //HandleCurrentSpeedChanged(_curSpeedData, 0, 0);
            //HandleAccelerationChanged(_accData, 0, 0);
        }


        

    }
}
