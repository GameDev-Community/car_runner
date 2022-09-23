#if UNITY_EDITOR
using DevourDev.Base.Reflections;
using Externals.Utils.Items;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Car
{
    public class WheelsOnSceneInitializer : MonoBehaviour
    {
        [SerializeField] private CustomizableCar _customizableCar;
        [SerializeField] private bool _init;


        private void OnValidate()
        {
            if (UnityEngine.Application.isPlaying)
                return;

            if (_init)
            {
                _init = false;
                Init();
                var go = gameObject;

                /*Destroy may not be called from edit mode! Use
                 * DestroyImmediate instead. */
                //Destroy(this);

                /* Destroying components immediately is not permitted during
                 * physics trigger/contact, animation event callbacks, rendering
                 * callbacks or OnValidate. You must use Destroy instead. */
                //DestroyImmediate(this);

                EditorUtility.SetDirty(go);
            }
        }

        private void Init()
        {
            var controller = _customizableCar.GetFieldValue<CarController>("_carController");
            var decorator = _customizableCar.GetFieldValue<CarDecorator>("_carDecorator");
            var wts = decorator.WheelTransforms;
            var wcs = controller.WheelColliders;
            var wtsC = wts.Length;
            var itemsType = _customizableCar.GetFieldValue<ItemType>("_wheelType");
            var wheelBehs = new WheelItemBehaviour[wtsC];

            for (int i = -1; ++i < wtsC;)
            {
                var wheelTr = wts[i];
                var wbGO = wheelTr.gameObject;

                if (!wbGO.TryGetComponent<WheelItemBehaviour>(out var wheelBeh))
                {
                    wheelBeh = wbGO.AddComponent<WheelItemBehaviour>();
                }

                wheelBeh.SetField("_wheelCollider", wcs[i]);
                wheelBeh.SetInheritedField(typeof(DefualtItemBehaviour), "_itemType", itemsType);
                wheelBehs[i] = wheelBeh;

                EditorUtility.SetDirty(wheelBeh.gameObject);
            }

            var itemsOnScene = new WheelsOnScene(itemsType, wheelBehs);
            _customizableCar.SetField("_wheelsCustomizables", itemsOnScene);
        }

        //private void InitOld()
        //{
        //    var controller = _customizableCar.GetFieldValue<CarController>("_carController");
        //    var decorator = _customizableCar.GetFieldValue<CarDecorator>("_carDecorator");
        //    var wts = decorator.WheelTransforms;
        //    var wtsC = wts.Length;
        //    var pts = new ItemsOnScene<WheelItemBehaviour>[wtsC];

        //    for (int i = -1; ++i < wtsC;)
        //    {
        //        var tr = wts[i];
        //        var pointGO = new GameObject($"{tr.gameObject.name} point");
        //        Transform point = pointGO.transform;
        //        var wc = controller.WheelColliders[i];

        //        point.SetParent(tr, false);
        //        point.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        //        point.SetParent(tr.parent, true);
        //        tr.SetParent(point);
        //        WheelItemBehaviour wheelBeh = tr.gameObject.AddComponent<WheelItemBehaviour>();
        //        wheelBeh.SetField("_wheelCollider", wc);

        //        ItemsOnScene<WheelItemBehaviour>.ItemPoint ip = new(point, wheelBeh);
        //        pts[i] = ip;
        //        EditorUtility.SetDirty(pointGO);
        //        EditorUtility.SetDirty(tr.gameObject);
        //    }

        //    ItemsOnScene<WheelItemBehaviour> ips = new(_customizableCar.GetFieldValue<ItemType>("_wheelType"), pts);
        //    _customizableCar.SetField("_wheelsCustomizables", ips);
        //    EditorUtility.SetDirty(_customizableCar.gameObject);
        //}

        //public void InitWheels(CarDecorator decorator)
        //{
        //    ItemType itemsType = ItemType;
        //    var wts = decorator.WheelTransforms;
        //    var wtsC = wts.Length;
        //    ItemPoint[] points = new ItemsPoints<WheelItemBehaviour>.ItemPoint[wtsC];

        //    for (int i = -1; ++i < wtsC;)
        //    {
        //        var p = points[i];
        //        p.Point = instanti
        //    }
        //}
    }
}
#endif