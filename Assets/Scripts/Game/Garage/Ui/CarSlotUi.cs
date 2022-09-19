using Externals.Utils.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Garage.Ui
{
    public class UpgradeSlotUi : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _img;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _activeTierText;


        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }



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
}
