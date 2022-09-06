using Externals.Utils.Runtime;
using Game.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Garage.Unsorted
{
    public class CarSlotUi : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _carNameText;
        [SerializeField] private Image _previewImg;

        private CarObject _carObject;
        private GarageUi _parent;
        private Sprite _tmpSprite;


        public CarObject CarObject => _carObject;


        public void Init(CarObject carObject, GarageUi parent)
        {
            _carObject = carObject;
            _parent = parent;

            var info = carObject.MetaInfo;
            _carNameText.text = info.Name;
            _previewImg.sprite = _tmpSprite = DevourRuntimeHelpers.SpriteFromTexture(info.HiresTex);
        }

        private void OnDestroy()
        {
            if (_tmpSprite != null)
                Destroy(_tmpSprite);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _parent.HandleCarSlotClick(this);
        }
    }
    public class GarageUi : MonoBehaviour
    {
        [SerializeField] private CarSlotUi _carSlotPrefab;
        [SerializeField] private Transform _carSlotsParent;

        private readonly List<CarSlotUi> _carSlots = new();
        private GarageData _garageData;


        private void BuildCarSlots(CarObject[] cars)
        {
            DestroyCarSlots();

            var c = cars.Length;

            for (int i = -1; ++i < c;)
            {
                var slot = Instantiate(_carSlotPrefab, _carSlotsParent);
                slot.Init(cars[i], this);
                _carSlots.Add(slot);
            }
        }

        private void DestroyCarSlots()
        {
            var arr = _carSlots;
            var c = arr.Count;

            if (c == 0)
                return;


            for (int i = -1; ++i < c;)
            {
                Destroy(arr[i].gameObject);
            }

            arr.Clear();
        }


        internal void HandleCarSlotClick(CarSlotUi slot)
        {
            //if()
            //var wallet = Accessors.Wallet;


        }
    }
    public class GarageMaster : MonoBehaviour
    {

    }
}
