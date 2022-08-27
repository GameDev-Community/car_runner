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
        [SerializeField] private bool _saveRatio;


        public ClampedFloatStatDataCreator(StatObject statObject, float min, float max, float initial, bool saveRatio)
        {
            _statObject = statObject;
            _min = min;
            _max = max;
            _initial = initial;
            _saveRatio = saveRatio;
        }


        public StatObject StatObject => _statObject;


        public ClampedFloatStatData Create()
            => new(_statObject, _min, _max, _initial, _saveRatio);
    }
}
