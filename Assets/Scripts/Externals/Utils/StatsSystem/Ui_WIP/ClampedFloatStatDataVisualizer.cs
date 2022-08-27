using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Externals.Utils.StatsSystem.Ui
{
    public class ValueVisualizer : MonoBehaviour
    {
        [SerializeField] private Image _iconImg;
        [SerializeField] private TextMeshProUGUI _valueText;

        private Sprite _tmpSprite;


        public void SetValueText(string value)
        {
            _valueText.text = value;
        }

        public void RemoveIcon()
        {
            if (_iconImg != null)
                Destroy(_iconImg.gameObject);
#if UNITY_EDITOR
            else
                Debug.LogError("attempt to remove already removed component");
#endif
        }

        public void SetIcon(Sprite iconSprite)
        {
            if (_iconImg == null)
#if UNITY_EDITOR
                throw new System.NullReferenceException(nameof(_iconImg));
#else
                return;
#endif

            _iconImg.sprite = iconSprite;
        }

        public void SetIcon(Texture2D iconTex)
        {
            if (_iconImg == null)
#if UNITY_EDITOR
                throw new System.NullReferenceException(nameof(_iconImg));
#else
                return;
#endif

            UpdateTmpSprite(Sprite.Create(iconTex,
                Rect.MinMaxRect(0, 0, iconTex.width, iconTex.height),
                new Vector2(0.5f, 0.5f)));

            _iconImg.sprite = _tmpSprite;
        }

        private void UpdateTmpSprite(Sprite sprite)
        {
            if (_tmpSprite != null)
                DestroyImmediate(_tmpSprite);

            _tmpSprite = sprite;
        }

    }

    public class ClampedValueVisualizer : ValueVisualizer
    {
        [SerializeField] private TextMeshProUGUI _minValueText;
        [SerializeField] private TextMeshProUGUI _maxValueText;


        public void RemoveMinValueText()
        {
            if (_minValueText != null)
                Destroy(_minValueText.gameObject);
#if UNITY_EDITOR
            else
                Debug.LogError("attempt to remove already removed component");
#endif
        }

        public void SetMinValueText(string value)
        {
            if (_minValueText == null)
#if UNITY_EDITOR
                throw new System.NullReferenceException(nameof(_minValueText));
#else
                return;
#endif
            _minValueText.text = value;
        }
        public void SetMaxValueText(string value)
        {
            _maxValueText.text = value;
        }


    }
    public class ClampedFloatStatDataVisualizer : StatVisualizerBase<ClampedFloatStatData>
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


        private void HandleClampedValueChanged(ClampedFloat se, float arg2, float arg3)
        {
            _curText.text = se.Value.ToString("N1");
            RaiseUnityEvents();
        }

        private void HandleBoundsChanged(ClampedFloat se, Vector3 arg2)
        {
            var sd = StatData;
            var min = sd.Min;
            var max = sd.Max;
            var cur = sd.Value;
            _minText.text = min.ToString("N1");
            _maxText.text = max.ToString("N1");
            _curText.text = cur.ToString("N1");
            RaiseUnityEvents();
        }


        private void RaiseUnityEvents()
        {
            _onValueChanged?.Invoke();
            var sd = StatData;
            var min = sd.Min;
            var max = sd.Max;
            var cur = sd.Value;
            var normalized = Mathf.InverseLerp(min, max, cur);
            _onRatioChanged_normalizedValue?.Invoke(normalized);
        }
    }
}
