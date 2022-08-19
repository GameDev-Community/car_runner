using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    [CreateAssetMenu(menuName = "Stats System/Stat Object")]
    public class StatObject : ScriptableObject
    {
        [SerializeField] MetaInfo _metaInfo;


        public MetaInfo MetaInfo => _metaInfo;
    }
}
