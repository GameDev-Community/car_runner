using UnityEngine;

namespace Utils
{
    public class SquareBoundedItemDefault : MonoBehaviour, ISquareBoundedItem
    {
        [SerializeField] private Component _prefab;
        [SerializeField] private SquareBounds _squareBounds;


        public SquareBounds SquareBounds => _squareBounds;


        public Component GetItemInstance()
        {
            return GameObject.Instantiate(_prefab);
        }

        public Component GetItemInstance(Transform parent)
        {
            return GameObject.Instantiate(_prefab, parent);
        }
    }

}