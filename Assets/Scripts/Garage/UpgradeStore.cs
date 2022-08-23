using UnityEngine;

namespace Garage
{
    internal sealed class UpgradeStore : MonoBehaviour
    {
        [SerializeField] private Transform upgradeSlotsHolder;
        [SerializeField] private UpgradeSlot upgradeSlotPrefab;

        public void InitializeUpgradeSlots(CarInfo carInfo)
        {
            DestroyUpgradeSlots();

            for (int i = 0; i < carInfo.CarUpgradeElements.Length; i++)
            {
                UpgradeSlot upgradeSlot = Instantiate(upgradeSlotPrefab, upgradeSlotsHolder);
                upgradeSlot.Initialize(this, carInfo.CarUpgradeElements[i]);
            }
        }

        public void DestroyUpgradeSlots()
        {
            for (int i = 0; i < upgradeSlotsHolder.childCount; i++)
            {
                Destroy(upgradeSlotsHolder.GetChild(i).gameObject);
            }
        }

        public void TryBuyUpgrade(UpgradeSlot upgradeSlot, UpgradeElementInfo upgradeElementInfo)
        {
            // upgradeElementInfo.Price
            // попытка купить и сохранение куда-то
        }
    }
}