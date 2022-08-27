using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public sealed class ClampedIntStatDataCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private int _min;
        [SerializeField] private int _max;
        [SerializeField] private int _initial;
        [SerializeField] private bool _saveRatio;


        public ClampedIntStatDataCreator(StatObject statObject, int min, int max, int initial, bool saveRatio)
        {
            _statObject = statObject;
            _min = min;
            _max = max;
            _initial = initial;
            _saveRatio = saveRatio;
        }


        public StatObject StatObject => _statObject;


        public ClampedIntStatData Create()
            => new(_statObject, _min, _max, _initial, _saveRatio);
    }
}
