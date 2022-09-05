#if UNITY_EDITOR
using DevourDev.Base.Reflections;
using Externals.Utils;
using Externals.Utils.Runtime;
using Externals.Utils.StatsSystem;
using System.IO;
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
            carObj.SetField(nameof(_upgrades), _upgrades);

            var arr = _upgrades;
            int c = arr.Length;

            if (c > 0)
            {
                var upgradesAbsFolderPath = new DirectoryInfo(pathToSaveAsset).Parent;
                var upgradesFolderPath = DevourRuntimeHelpers.AbsPathToUnityRelative(upgradesAbsFolderPath);
                var upgradesFolderName = Path.GetFileNameWithoutExtension(pathToSaveAsset) + "_upgrades";
                var upgradesFolderFullName = Path.Combine(upgradesFolderPath, upgradesFolderName);
                AssetDatabase.CreateFolder(upgradesFolderPath, upgradesFolderName);
                //upgsPath = AssetDatabase.GUIDToAssetPath(upgsPath);


                for (int i = -1; ++i < c;)
                {
                    var ass = arr[i];
                    //AssetDatabase.CreateAsset(ass, Path.Combine(upgsPath, $"upg_{i}.asset"));
                    AssetDatabase.CreateAsset(ass, Path.Combine(upgradesFolderFullName, $"upg_{i}.asset"));
                    EditorUtility.SetDirty(ass);
                }
            }


            AssetDatabase.CreateAsset(carObj, pathToSaveAsset);
            EditorUtility.SetDirty(carObj);

            return carObj;
        }
    }
}
#endif