#if UNITY_EDITOR
using DevourDev.Base.Reflections;
using Externals.Utils;
using Externals.Utils.StatsSystem;
using UnityEditor;
using UnityEngine;

namespace Game.Garage
{

    public class CarCreator
    {
        private MetaInfo _metaInfo;
        private StatDataRuntimeCreator[] _sourceStats;
        private UpgradeObject[] _upgrades;


        public MetaInfo MetaInfo { get => _metaInfo; set => _metaInfo = value; }
        public StatDataRuntimeCreator[] SourceStats { get => _sourceStats; set => _sourceStats = value; }
        public UpgradeObject[] Upgrades { get => _upgrades; set => _upgrades = value; }


        public CarObject Create(string pathToSaveAsset)
        {
            var carObj = ScriptableObject.CreateInstance<CarObject>();
            carObj.SetField(nameof(_metaInfo), _metaInfo);
            carObj.SetField(nameof(_sourceStats), _sourceStats);
            //carObj.SetField(nameof(_upgrades), _upgrades);
            AssetDatabase.CreateAsset(carObj, pathToSaveAsset);
            EditorUtility.SetDirty(carObj);
            return carObj;
        }
    }
}
#endif