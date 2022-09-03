using System.Collections.Generic;
using UnityEngine;

namespace Utils.Items.Customizables
{
    [System.Serializable]
    public class ItemsOnScene<TItem> where TItem : DefualtItemBehaviour
    {
        //[System.Serializable]
        //public class ItemPoint
        //{
        //    [SerializeField] private Transform _point;
        //    [SerializeField] private TItem _currentItem;


        //    public ItemPoint(Transform point, TItem currentItem)
        //    {
        //        _point = point;
        //        _currentItem = currentItem;
        //    }


        //    public Transform Point => _point;

        //    public TItem CurrentItem { get => _currentItem; set => _currentItem = value; }
        //}


        /// <summary>
        /// Sender, old, olds count, new, news count.
        /// news are references (prefabs) probably.
        /// Event raises before any changes.
        /// !!! ALL ARRAYS ARE RENTED AND WILL
        /// BE RETURNED AFTER ALL LISTENERS
        /// HAVE HANDLED THE EVENT !!!
        /// Значения одного массива не обязательно должны быть
        /// одинаковыми, равно как и количество старых и новых.
        /// </summary>
        public event System.Action<ItemsOnScene<TItem>, TItem[], int, TItem[], int> OnBeforeChange;
        /// <summary>
        /// Sender, news.
        /// News are instances.
        /// !!! Array is REFERENCE !!!
        /// </summary>
        public event System.Action<ItemsOnScene<TItem>, TItem[]> OnChanged;


        [SerializeField] private ItemType _itemsType;
        //[SerializeField] private ItemPoint[] _points;
        [SerializeField] private TItem[] _items;


        //public ItemsPoints(ItemType itemsType, ItemPoint[] points)
        public ItemsOnScene(ItemType itemsType, TItem[] items)
        {
            _itemsType = itemsType;
            //_points = points;
            _items = items;
        }


        public ItemType ItemType => _itemsType;
        //public ItemPoint[] Points => _points;
        public TItem[] Items { get => _items; protected set => _items = value; }


        //public System.Action<TItem> BeforeRemoveOldItemAction { get; set; }
        //public System.Action<TItem> AfterAddNewItemAction { get; set; }


        //protected void InitInternal(ItemType itemsType, ItemPoint[] points)
        protected void InitInternal(ItemType itemsType, TItem[] items)
        {
            _itemsType = itemsType;
            _items = items;
        }


        public void ChangeItems(TItem newItem)
        {
            if (newItem.ItemType != _itemsType)
            {
                UnityEngine.Debug.LogError("Attempt to change" +
                    $" items of different types ({newItem.ItemType}, {_itemsType})");
            }

            if (OnBeforeChange != null)
            {
                //var arr = _points;
                var arr = _items;
                var c = arr.Length;
                var pool = System.Buffers.ArrayPool<TItem>.Shared;
                var olds = pool.Rent(c);

                for (int i = -1; ++i < c;)
                {
                    //olds[i] = arr[i].CurrentItem;
                    olds[i] = arr[i];
                }

                var news = pool.Rent(c);

                for (int i = -1; ++i < c;)
                {
                    news[i] = newItem;
                }

                //без проверки на null, т.к. она была выше
                OnBeforeChange!.Invoke(this, olds, c, news, c);

                #region why not clearing
                //Объекты Юнити не подвергаются жмышарповскому собирателю
                //говна (манагаются из-под нативки) => чистить массив
                //бесполезно, но если ограничения этого класса изменятся
                // - нужно будет проверять тип на managed/unmanaged
                //System.Array.Clear(olds, 0, c);
                //System.Array.Clear(news, 0, c);
                #endregion

                pool.Return(olds, false);
                pool.Return(news, false);
            }

            #region lined
            //foreach (var p in _points)
            //{
            //    var ci = p.CurrentItem;
            //    if (ci != null)
            //    {
            //        BeforeRemoveOldItemAction?.Invoke(ci);
            //        UnityEngine.GameObject.Destroy(ci.gameObject);
            //    }

            //    var newCi = UnityEngine.GameObject.Instantiate(newItem, p.Point);
            //    p.CurrentItem = newCi;
            //    AfterAddNewItemAction?.Invoke(newCi);
            //}
            #endregion

            HandleChanging(newItem);

            OnChanged?.Invoke(this, _items);

        }

        protected virtual void HandleChanging(TItem newItem)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                TItem item = _items[i];
                //BeforeRemoveOldItemAction?.Invoke(item);
                Transform oldTr = item.transform;
                Transform parent = oldTr.parent;
                Vector3 pos = oldTr.localPosition;
                Quaternion rot = oldTr.localRotation;
                UnityEngine.GameObject.Destroy(item.gameObject);

                var newItemInst = UnityEngine.GameObject.Instantiate(newItem, parent);
                var newTr = newItemInst.transform;
                newTr.localPosition = pos;
                newTr.localRotation = rot;
                _items[i] = newItemInst;
                //AfterAddNewItemAction?.Invoke(newItemInst);
            }
        }

        public static Dictionary<ItemType, ItemsOnScene<TItem>> ToDictionary(ItemsOnScene<TItem>[] arr)
        {
            int c = arr.Length;
            Dictionary<ItemType, ItemsOnScene<TItem>> dic = new(c);

            for (int i = -1; ++i < c;)
            {
                var ip = arr[i];
                dic.Add(ip.ItemType, ip);
            }

            return dic;
        }
        public static Dictionary<ItemType, ItemsOnScene<TItem>> ToDictionary(ItemsOnScene<TItem>[][] arrs)
        {
            int arrsC = arrs.Length;
            int c = 0;

            for (int i = 0; i < arrsC; i++)
            {
                c += arrs[i].Length;
            }

            Dictionary<ItemType, ItemsOnScene<TItem>> dic = new(c);

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
