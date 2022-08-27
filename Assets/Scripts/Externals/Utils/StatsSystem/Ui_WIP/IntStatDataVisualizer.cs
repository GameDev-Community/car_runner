using UnityEngine;
using UnityEngine.Events;

namespace Externals.Utils.StatsSystem.Ui
{
    public class IntStatDataVisualizer : StatVisualizerBase<IntStatData>
    {
        [SerializeField] private TMPro.TextMeshProUGUI _valueText;
        [SerializeField] private UnityEvent _onValueChanged;


        protected override void HandleInitialization()
        {
            var sd = StatData;
            sd.OnValueChanged += HandleValueChanged;
        }


        private void HandleValueChanged(IValueCallback<int> se, int arg2)
        {
            _valueText.text = se.Value.ToString();
            _onValueChanged?.Invoke();
        }
    }
}
