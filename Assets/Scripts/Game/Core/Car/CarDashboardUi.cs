using UnityEngine;

namespace Game.Core.Car
{
    public class CarDashboardUi : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _avgRpmText;


        public void SetAvgRpm(float v)
        {
            _avgRpmText.text = v.ToString("N0");
        }
    }
}
