using UnityEngine;

namespace Utils
{
    public class SquareBoundedItem<T> : MonoBehaviour, ISquareBoundedItem
        where T : Component
    {
        [SerializeField] private T _prefab;
        [SerializeField] private SquareBounds _squareBounds;


        public SquareBounds SquareBounds => _squareBounds;


        public T Prefab => _prefab;


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