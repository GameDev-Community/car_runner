using DevourDev.Unity.ScriptableObjects;
using Externals.Utils;
using Externals.Utils.StatsSystem;
using UnityEngine;

namespace Game.Garage
{
    [CreateAssetMenu(menuName = "Game/Garage/Cars/Car Object")]
    public class CarObject : GameDatabaseElement
    {
        [SerializeField] private MetaInfo _metaInfo;
        [SerializeField] private StatDataRuntimeCreator[] _sourceStats;
        [SerializeField] private GameObject _carPreviewPrefab;
        [SerializeField] private UpgradeObject[] _upgrades;
        [SerializeField] private int _cost;


        public MetaInfo MetaInfo => _metaInfo;


        public object CreateCar()
        {
            //returns Instance
            throw new System.NotImplementedException();
        }
    }
     
}
