using Externals.Utils;
using UnityEngine;

namespace Utils.Items
{
    [CreateAssetMenu(menuName = "Items/Item Type Object")]
    public class ItemType : DevourDev.Unity.ScriptableObjects.GameDatabaseElement
    {
        [SerializeField] private MetaInfo _metaInfo;


        public MetaInfo MetaInfo => _metaInfo;
    }


}
