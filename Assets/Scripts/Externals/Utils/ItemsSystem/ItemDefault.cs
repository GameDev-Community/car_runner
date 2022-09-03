using UnityEngine;

namespace Utils.Items
{
    public class ItemDefault : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;


        public ItemType ItemType => _itemType;


        public virtual void InitItem(ItemType type)
        {
            _itemType = type;
        }
    }
}
