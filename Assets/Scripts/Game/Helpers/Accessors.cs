using Externals.Utils.StatsSystem;
using Game.Core;
using UnityEngine;

namespace Game.Helpers
{
    public class Accessors : MonoBehaviour
    {
        //[System.Serializable]
        //private struct StatObjects
        //{
        //    public StatObject Speed;
        //    public StatObject Acceleration;
        //    public StatObject Money;
        //    public StatObject Health;
        //    public StatObject Rockets;
        //    public StatObject Nitro;
        //}


        //public enum MainStats
        //{
        //    Speed,
        //    Acceleration,
        //    Money,
        //    Health,
        //    Rockets,
        //    Nitro
        //}


        [SerializeField] private Player _player;
        //[SerializeField] private StatObjects _statObjects;

        private static Accessors _inst;


        public static Player Player => _inst._player;
        public static StatsCollection PlayerStats => Player.StatsHolder.StatsCollection;


        private void Awake()
        {
            _inst = this;
        }


//        public static TStatData GetMainStatData<TStatData>(MainStats mainStat)
//            where TStatData : IStatData
//        {
//#if UNITY_EDITOR
//            var x
//#else  
//                _
//#endif
//                = PlayerStats.TryGetStatDataT<TStatData>(StatObjectFromEnum(mainStat), out var res);

//#if UNITY_EDITOR
//            if (!x)
//                throw new System.Collections.Generic.KeyNotFoundException(mainStat.ToString());
//#endif
//            return res;
//        }

//        private static StatObject StatObjectFromEnum(MainStats ev)
//        {
//            var ss = _inst._statObjects;

//            return ev switch
//            {
//                MainStats.Speed => ss.Speed,
//                MainStats.Acceleration => ss.Acceleration,
//                MainStats.Money => ss.Money,
//                MainStats.Rockets => ss.Rockets,
//                MainStats.Nitro => ss.Nitro,
//                _ => throw new System.NotSupportedException("unexpected enum value: " + ev.ToString()),
//            };
//        }
    }
}