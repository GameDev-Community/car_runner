using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [System.Serializable]
    public sealed class IntStatDataCreator
    {
        [SerializeField] private StatObject _statObject;
        [SerializeField] private int _initial;


        public IntStatDataCreator(StatObject statObject, int initial)
        {
            _statObject = statObject;
            _initial = initial;
        }


        public StatObject StatObject => _statObject;


        public IntStatData Create()
            => new(_statObject, _initial);
    }
}
