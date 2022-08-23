using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    internal sealed class UpgradeSlot : MonoBehaviour
    {
        [SerializeField] private Image upgradeImage;
        [SerializeField] private TMP_Text upgradeName;
        [SerializeField] private TMP_Text upgradePrice;

        private UpgradeStore upgradeStore;
        private CarUpgradeElement carUpgradeElement;
        private UpgradeElementInfo nextUpgradeElementInfo;

        public void Initialize(UpgradeStore upgradeStore, CarUpgradeElement carUpgradeElement) // нужна загрузка из сохранений
        {
            this.upgradeStore = upgradeStore;
            this.carUpgradeElement = carUpgradeElement;
            upgradeName.text = carUpgradeElement.Name;
            upgradeImage.sprite = carUpgradeElement.UISprite;

            if (carUpgradeElement.UpgradeElementInfos.Length > 1)
            {
                nextUpgradeElementInfo = carUpgradeElement.UpgradeElementInfos[1]; // нужна загрузка сохранений для выгрузки правильного уровня
                upgradePrice.text = nextUpgradeElementInfo.Price.ToString(); // выгрузка цены
            }
        }

        public void OnUpgradeButtonPressed()
        {
            upgradeStore.TryBuyUpgrade(this, nextUpgradeElementInfo);
        }
    }
}