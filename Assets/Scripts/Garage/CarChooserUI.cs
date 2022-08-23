using UnityEngine;
using UnityEngine.UI;

namespace Garage
{
    internal sealed class CarChooserUI : MonoBehaviour
    {
        [SerializeField] private Button rightSwitchButton;
        [SerializeField] private Button leftSwitchButton;

        public void ChangeRightSwitchButtonState(bool state)
        {
            if (rightSwitchButton.interactable != state)
                rightSwitchButton.interactable = state;
        }

        public void ChangeLeftSwitchButtonState(bool state)
        {
            if (leftSwitchButton.interactable != state)
                leftSwitchButton.interactable = state;
        }
    }
}