using UnityEngine;

namespace Utils.Items
{
    [CreateAssetMenu(menuName = "Items/Item Types Database")]
    public class ItemTypesDatabase : DevourDev.Unity.ScriptableObjects.GameDatabase<ItemType>
    {
        private void OnValidate()
        {
            ManageElementsOnValidate();
        }
    }
}
