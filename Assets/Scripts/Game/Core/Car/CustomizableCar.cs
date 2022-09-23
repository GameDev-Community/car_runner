using Externals.Utils.Items;
using Externals.Utils.Items.Customizables;
using UnityEngine;

namespace Game.Core.Car
{
    public class CustomizableCar : MonoBehaviour, ICustomizablesOwner<DefualtItemBehaviour>
    {
        [SerializeField] private CarController _carController;
        [SerializeField] private CarDecorator _carDecorator;
        [SerializeField] private ItemType _wheelType;
        [SerializeField] private WheelsOnScene _wheelsCustomizables;


        private void Awake()
        {
            //_wheelsCustomizables.OnBeforeChange += WheelsCustomizables_OnChange;
            _wheelsCustomizables.OnChanged += HandleWheelsChanged;
        }

        private void HandleWheelsChanged(ItemsOnScene<WheelItemBehaviour> sender, WheelItemBehaviour[] wheels)
        {
            WheelCollider[] wcs = _carController.WheelColliders;
            Transform[] wts = _carDecorator.WheelTransforms;
            var c = wheels.Length;

            for (int i = -1; ++i < c;)
            {
                wcs[i] = wheels[i].WheelCollider;
                wts[i] = wheels[i].transform;
            }
        }

        //private void WheelsCustomizables_OnChange(ItemsOnScene<WheelItemBehaviour> sender, WheelItemBehaviour[] olds, int oldsC, WheelItemBehaviour[] news, int newsC)
        //{
        //    WheelCollider[] wcs = _carController.WheelColliders;
        //    Transform[] wts = _carDecorator.WheelTransforms;

        //    for (int i = -1; ++i < newsC;)
        //    {
        //        wcs[i] = news[i].WheelCollider;
        //        wts[i] = news[i].transform;
        //    }
        //}

        public void ChangeItem(DefualtItemBehaviour newItem)
        {
            if (newItem.ItemType == _wheelType)
            {
                if (newItem is not WheelItemBehaviour newWheelsItem)
                {
                    UnityEngine.Debug.LogError("attempt to add new wheels, but they are not of type " + nameof(WheelItemBehaviour));
                    return;
                }

                _wheelsCustomizables.ChangeItems(newWheelsItem);
            }
        }
    }
}
