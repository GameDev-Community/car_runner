using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class RandomItem<T>
    {
        [SerializeField] private readonly T _item;
        [SerializeField, Range(0, 10_000)] private int _chance;


        public T Item => _item;
        public int Chance => _chance;
    }

    public class RandomUnityObject<TUnityObject> : RandomItem<TUnityObject> where TUnityObject : Object
    {
    }
}