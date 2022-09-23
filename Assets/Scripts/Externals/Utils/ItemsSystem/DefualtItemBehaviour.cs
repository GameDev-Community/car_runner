using UnityEngine;

namespace Externals.Utils.Items
{
    public class DefualtItemBehaviour : MonoBehaviour, IItem
    {
        [SerializeField] private ItemType _itemType;


        public ItemType ItemType => _itemType;


        public virtual void InitItem(ItemType type)
        {
            _itemType = type;
        }
    }
}
