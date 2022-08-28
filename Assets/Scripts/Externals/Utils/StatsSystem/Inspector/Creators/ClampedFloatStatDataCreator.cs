using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public sealed class ClampedFloatStatDataCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        [SerializeField] private float _initial;


        public ClampedFloatStatDataCreator(StatObject statObject, float min, float max, float initial)
        {
            _statObject = statObject;
            _min = min;
            _max = max;
            _initial = initial;
        }


        public StatObject StatObject => _statObject;


        public ClampedFloatStatData Create()
            => new(_statObject, _min, _max, _initial);
    }
}
