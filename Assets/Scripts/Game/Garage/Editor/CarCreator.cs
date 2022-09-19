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
            const string upgradesFolderName = "Upgrades";
            const string assetFielExtention = ".asset";

            var carObj = ScriptableObject.CreateInstance<CarObject>();
            carObj.SetField(nameof(_metaInfo), _metaInfo);
            carObj.SetField(nameof(_sourceStats), _sourceStats);
            carObj.SetField(nameof(_upgrades), _upgrades);

            string carFileName = Path.GetFileNameWithoutExtension(pathToSaveAsset);
            string contentRelativeFolderPath = DevourRuntimeHelpers.CatalogUp(pathToSaveAsset, 1);
            contentRelativeFolderPath = Path.Combine(contentRelativeFolderPath, carFileName);
            _ = Directory.CreateDirectory(contentRelativeFolderPath); //need only create directory (file system structure)
            var arr = _upgrades;
            int c = arr.Length;

            if (c > 0)
            {
                DirectoryInfo upgradesDirectory = new(Path.Combine(contentRelativeFolderPath, upgradesFolderName));
                upgradesDirectory.Create();
                var upgradesRelativeFolderPath = DevourRuntimeHelpers.AbsPathToUnityRelative(upgradesDirectory);

                for (int i = -1; ++i < c;)
                {
                    var ass = arr[i];
                    AssetDatabase.CreateAsset(ass, Path.Combine(upgradesRelativeFolderPath, $"upg_{i}{assetFielExtention}"));
                    EditorUtility.SetDirty(ass);
                }
            }


            AssetDatabase.CreateAsset(carObj, Path.Combine(contentRelativeFolderPath, carFileName + assetFielExtention));
            EditorUtility.SetDirty(carObj);

            return carObj;
        }
    }
}
#endif