using DevourDev.Unity.ScriptableObjects;
using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [CreateAssetMenu(menuName = "Stats System/Stat Object")]
    public class StatObject : GameDatabaseElement
    {
        public enum NumericsType
        {
            Integer,
            Floating
        }


        [System.Serializable]
        public struct StatDataInfo
        {
            public NumericsType NumericsType;
            public bool Clamped;
            public bool SaveRatio;
            public bool Modifiable;
        }


        [SerializeField] MetaInfo _metaInfo;
        [SerializeField] private StatDataInfo _statDataInfo;



        public MetaInfo MetaInfo => _metaInfo;


        public StatDataInfo GetStatDataInfo()
            => _statDataInfo;
    }

}
