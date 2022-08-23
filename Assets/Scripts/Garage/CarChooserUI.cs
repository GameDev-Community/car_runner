using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    internal sealed class CarChooserUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text carNameText;
        [SerializeField] private TMP_Text carPrice;
        [SerializeField] private Button buyCarButton;
        [SerializeField] private Button nextCarButton;
        [SerializeField] private Button previousCarButton;

        public void ChangeNextCarButtonState(bool state)
        {
            if (nextCarButton.interactable != state)
                nextCarButton.interactable = state;
        }

        public void ChangePreviousCarButtonState(bool state)
        {
            if (previousCarButton.interactable != state)
                previousCarButton.interactable = state;
        }

        public void UpdateCarInfo(CarInfo carInfo)
        {
            carNameText.text = carInfo.Name;
            carPrice.text = carInfo.Price.ToString(); // need check on already buy
            buyCarButton.interactable = true; // need check enough money for buy or set false
        }
    }
}