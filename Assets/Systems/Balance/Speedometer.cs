using UnityEngine;

namespace Game.Core
{
    public class Speedometer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _speedVText;
        [SerializeField] private TMPro.TextMeshProUGUI _accelVText;


        public void SetSpeed(float v)
        {
            _speedVText.text = $"скорость: {v:N1}";
        }

        public void SetAcceleration(float v)
        {
            _accelVText.text = $"ускорение: {v:N1}";
        }
    }
}
