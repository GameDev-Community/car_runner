using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    internal sealed class CarChooser : MonoBehaviour
    {
        [SerializeField] private CarModelsPreview carModelsPreview;
        [SerializeField] private CarChooserUI carChooserUI;
        [SerializeField] private CarInfo currentCar;
        [SerializeField] private CarInfo[] carInfos;

        private int carIndex = 0;
        
        private void OnEnable()
        {
            carModelsPreview.InitializeCars(carInfos, currentCar);
            CheckNeedLockSwitchButtons();
        }

        private void OnDisable()
        {
            carModelsPreview.DestroyCarModels();
        }

        public void ToNextCar()
        {
            currentCar = carInfos[++carIndex];
            CheckNeedLockSwitchButtons();
            carModelsPreview.Move(-1);
        }

        public void ToPreviousCar()
        {
            currentCar = carInfos[--carIndex];
            CheckNeedLockSwitchButtons();
            carModelsPreview.Move(1);
        }

        private void CheckNeedLockSwitchButtons()
        {
            carChooserUI.ChangeRightSwitchButtonState(carIndex != carInfos.Length - 1);
            carChooserUI.ChangeLeftSwitchButtonState(carIndex != 0);
        }
    }
}