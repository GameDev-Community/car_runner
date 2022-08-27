using UnityEngine;
using UnityEngine.Events;

namespace Externals.Utils.StatsSystem.Ui
{
    public class FloatStatDataVisualizer : StatVisualizerBase<FloatStatData>
    {
        [SerializeField] private TMPro.TextMeshProUGUI _valueText;
        [SerializeField] private UnityEvent _onValueChanged;


        protected override void HandleInitialization()
        {
            var sd = StatData;
            sd.OnValueChanged += HandleValueChanged;
        }


        private void HandleValueChanged(IValueCallback<float> se, float arg2)
        {
            _valueText.text = se.Value.ToString("N1");
            _onValueChanged?.Invoke();
        }
    }
}
