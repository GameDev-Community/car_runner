using DevourDev.Unity.ScriptableObjects;
using Externals.Utils;
using Externals.Utils.StatsSystem;
using Externals.Utils.StatsSystem.Modifiers;
using UnityEngine;

namespace Game.Garage
{

    [CreateAssetMenu(menuName = "Game/Garage/Cars/Car Object")]
    public class CarObject : GameDatabaseElement    {
        [SerializeField] private MetaInfo _metaInfo;
        [SerializeField] private StatDataRuntimeCreator[] _sourceStats; //ahaiasidasodaspphooooooi
        [SerializeField] private GameObject _carPreviewPrefab;
        [SerializeField] private UpgradeData[] _upgrades;
        [SerializeField] private int _statsCount;


        public MetaInfo MetaInfo => _metaInfo;


        private void OnValidate()
        {
            _statsCount = _sourceStats.Length;

            foreach (var item in _sourceStats)
            {
                var sd = item.Create();
                UnityEngine.Debug.Log(sd.GetType().ToString());

                UnityEngine.Debug.Log(sd.StatObject);

                if (sd is FloatModifiableStatData fmsd)
                {
                    UnityEngine.Debug.Log(fmsd.Value);
                }
                else if (sd is FloatDynamicStatData fdsd)
                {
                    UnityEngine.Debug.Log(fdsd.Value);
                }
            }
        }


        public object CreateCar()
        {
            //returns Instance
            throw new System.NotImplementedException();
        }
    }
}
