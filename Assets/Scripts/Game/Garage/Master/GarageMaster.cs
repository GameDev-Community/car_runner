using Externals.Utils.Runtime;
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

        private int _id;
        private GarageUi _parent;
        private Sprite _tmpSprite;


        public void Init(int id, GarageUi parent, CarObject carObject)
        {
            _id = id;
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
            _parent.HandleCarSlotClick(_id);
        }
    }
    public class GarageUi : MonoBehaviour
    {
        [SerializeField] private CarSlotUi _carSlotPrefab;
        [SerializeField] private Transform _carSlotsParent;
        private readonly List<CarSlotUi> _carSlots = new();


        private void BuildCarSlots(CarObject[] cars)
        {
            DestroyCarSlots();

            var c = cars.Length;

            for (int i = -1; ++i < c;)
            {
                var slot = Instantiate(_carSlotPrefab, _carSlotsParent);
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


        internal void HandleCarSlotClick(int slotIndex)
        {

        }
    }
    public class GarageMaster : MonoBehaviour
    {

    }
}
