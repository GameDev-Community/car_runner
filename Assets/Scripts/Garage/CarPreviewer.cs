using UnityEngine;

namespace Garage
{
    internal sealed class CarPreviewer : MonoBehaviour
    {
        [SerializeField] private CarModelsPreview carModelsPreview;
        [SerializeField] private CarChooserUI carChooserUI;
        [SerializeField] private UpgradeStore upgradeStore;
        [SerializeField] private CarInfo currentCar; // нужен для проверки симуляции загрузки не 0 машины
        [SerializeField] private CarInfo[] carInfos;

        private int carIndex = 0;

        private void OnEnable()
        {
            carModelsPreview.InitializeCars(carInfos, currentCar);            
            UpdatePreview();
        }

        private void OnDisable()
        {
            carModelsPreview.DestroyCarModels();
            upgradeStore.DestroyUpgradeSlots();
        }

        public void ToNextCar()
        {
            currentCar = carInfos[++carIndex];
            UpdatePreview();
            carModelsPreview.Move(-1);
        }

        public void ToPreviousCar()
        {
            currentCar = carInfos[--carIndex];
            UpdatePreview();
            carModelsPreview.Move(1);
        }

        public void TryBuyCar()
        {
            // currentCar.Price
            // попытка купить. 
        }

        private void UpdatePreview()
        {
            carChooserUI.ChangeNextCarButtonState(carIndex != carInfos.Length - 1);
            carChooserUI.ChangePreviousCarButtonState(carIndex != 0);
            carChooserUI.UpdateCarInfo(currentCar);

            upgradeStore.InitializeUpgradeSlots(currentCar);
        }
    }
}