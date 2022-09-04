using UnityEngine;
using Utils.Items;
using Utils.Items.Customizables;

namespace Game.Core.Car
{
    [System.Serializable]
    public class WheelsOnScene : ItemsOnScene<WheelItemBehaviour>
    {
        public WheelsOnScene(ItemType itemsType, WheelItemBehaviour[] items) : base(itemsType, items)
        {
        }

        protected override void HandleChanging(WheelItemBehaviour newItem)
        {
            var arr = Items;
            var c = arr.Length;

            for (int i = -1; ++i < c;)
            {
                var item = arr[i];

                var oldWC = item.WheelCollider;
                Transform oldWCTr = oldWC.transform;
                Transform wcParent = oldWCTr.parent;
                Vector3 wcPos = oldWCTr.localPosition;
                Quaternion wcRot = oldWCTr.localRotation;

                //если планируется менять хуйню во время заезда,
                //нужно еще перенести значения сил из старого говна
                //в новую хуйню

                GameObject.Destroy(item.WheelCollider.gameObject);

                var oldItemTr = item.transform;
                var itemParent = oldItemTr.parent;
                var itemPos = oldItemTr.localPosition;
                var itemRot = oldItemTr.localRotation;
                var itemScale = oldItemTr.localScale;

                GameObject.Destroy(item.gameObject);
                               
                var newItemInst = GameObject.Instantiate(newItem, itemParent, false);
                var newItemTr = newItemInst.transform;
                newItemTr.localPosition = itemPos;
                newItemTr.localRotation = itemRot;
                newItemTr.localScale = itemScale;
                var newWC = newItemInst.WheelCollider;
                var newWCTr = newWC.transform;
                newWCTr.SetParent(wcParent, false);
                newWCTr.localPosition = wcPos;
                newWCTr.localRotation = wcRot;

                arr[i] = newItemInst;
            }
        }
    }
}
