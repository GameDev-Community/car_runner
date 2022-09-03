using System.Collections.Generic;
using UnityEngine;

namespace Utils.Items
{
    [System.Serializable]
    public class ItemsPoints<TItem> where TItem : ItemDefault
    {
        [System.Serializable]
        public class ItemPoint
        {
            [SerializeField] private Transform _point;
            [SerializeField] private TItem _currentItem;


            public Transform Point => _point;

            public TItem CurrentItem { get => _currentItem; set => _currentItem = value; }
        }


        [SerializeField] private ItemType _itemsType;
        [SerializeField] private ItemPoint[] _points;


        public ItemType ItemType => _itemsType;
        public ItemPoint[] Points => _points;


        public System.Action<TItem> BeforeRemoveOldItemAction { get; set; }
        public System.Action<TItem> AfterAddNewItemAction { get; set; }


        public void ChangeItems(TItem newItem)
        {
            foreach (var p in _points)
            {
                var ci = p.CurrentItem;
                if (ci != null)
                {
                    BeforeRemoveOldItemAction?.Invoke(ci);
                    UnityEngine.GameObject.Destroy(ci.gameObject);
                }

                var newCi = UnityEngine.GameObject.Instantiate(newItem, p.Point);
                p.CurrentItem = newCi;
                AfterAddNewItemAction?.Invoke(newCi);
            }
        }


        public static Dictionary<ItemType, ItemsPoints<TItem>> ToDictionary(ItemsPoints<TItem>[] arr)
        {
            int c = arr.Length;
            Dictionary<ItemType, ItemsPoints<TItem>> dic = new(c);

            for (int i = -1; ++i < c;)
            {
                var ip = arr[i];
                dic.Add(ip.ItemType, ip);
            }

            return dic;
        }
        public static Dictionary<ItemType, ItemsPoints<TItem>> ToDictionary(ItemsPoints<TItem>[][] arrs)
        {
            int arrsC = arrs.Length;
            int c = 0;

            for (int i = 0; i < arrsC; i++)
            {
                c += arrs[i].Length;
            }

            Dictionary<ItemType, ItemsPoints<TItem>> dic = new(c);

            for (int i = -1; ++i < arrsC;)
            {
                var arr = arrs[i];
                var arrC = arr.Length;
                for (int j = -1; ++j < arrC;)
                {
                    var ip = arr[j];
                    dic.Add(ip.ItemType, ip);
                }
            }

            return dic;
        }

    }
}
