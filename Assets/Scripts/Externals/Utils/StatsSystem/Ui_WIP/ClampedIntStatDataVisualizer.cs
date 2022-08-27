using UnityEngine;
using UnityEngine.Events;

namespace Externals.Utils.StatsSystem.Ui
{
    public class ClampedIntStatDataVisualizer : StatVisualizerBase<ClampedIntStatData>
    {
        [SerializeField] private TMPro.TextMeshProUGUI _minText;
        [SerializeField] private TMPro.TextMeshProUGUI _maxText;
        [SerializeField] private TMPro.TextMeshProUGUI _curText;

        [SerializeField] private UnityEvent _onValueChanged;
        [SerializeField] private UnityEvent<float> _onRatioChanged_normalizedValue;


        protected override void HandleInitialization()
        {
            var sd = StatData;
            sd.OnClampedValueChanged += HandleClampedValueChanged;
            sd.OnBoundsChanged += HandleBoundsChanged;
        }


        private void HandleClampedValueChanged(ClampedInt se, int arg2, int arg3)
        {
            _curText.text = se.Value.ToString();
            RaiseUnityEvents();
        }

        private void HandleBoundsChanged(ClampedInt se, Vector3Int arg2)
        {
            var sd = StatData;
            var min = sd.Min;
            var max = sd.Max;
            var cur = sd.Value;
            _minText.text = min.ToString();
            _maxText.text = max.ToString();
            _curText.text = cur.ToString();
            RaiseUnityEvents();
        }


        private void RaiseUnityEvents()
        {
            _onValueChanged?.Invoke();
            var sd = StatData;
            float min = sd.Min;
            float max = sd.Max;
            float cur = sd.Value;
            var normalized = Mathf.InverseLerp(min, max, cur);
            _onRatioChanged_normalizedValue?.Invoke(normalized);
        }
    }
}
