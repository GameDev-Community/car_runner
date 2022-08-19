using UnityEngine;

namespace Externals.Utils.StatsSystem
{
    public class StatsHolder : MonoBehaviour, IStatsHolder
    {
        [SerializeField] private StatDataCreator[] _initialStats;

        private readonly StatsCollection _statsCollection = new();


        public StatsCollection StatsCollection => _statsCollection;


        private void Awake()
        {
            var sc = _statsCollection;
            var sdcs = _initialStats;

            foreach (var sdc in sdcs)
            {
              var check =  sc.TryAddStat(sdc);

#if UNITY_EDITOR
                if (!check)
                    throw new System.Exception("aгgumeнt ехсерti0n: "
                        + sdc.StatObject + " alгеаду ехист in collection");
#endif
            }
        }
    }
}
