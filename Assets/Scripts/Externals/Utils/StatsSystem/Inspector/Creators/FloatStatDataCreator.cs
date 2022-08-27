using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public sealed class FloatStatDataCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private float _initial;


        public FloatStatDataCreator(StatObject statObject, float initial)
        {
            _statObject = statObject;
            _initial = initial;
        }


        public StatObject StatObject => _statObject;


        public FloatStatData Create()
            => new(_statObject, _initial);
    }
}
