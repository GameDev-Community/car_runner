using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Testing.Shaders
{
    [RequireComponent(typeof(Canvas))]
    public sealed class CircleWipe : MonoBehaviour
    {
        [SerializeField, HideInInspector] private Canvas _canvas;
        [SerializeField, HideInInspector] private Image _img;
        [SerializeField, HideInInspector] private RectTransform _canvasRectTransfrom;

        private static CircleWipe _inst;
        private static Transform _focusTarget;
        private static Vector2 _targetScreenPos;

        //todo:

        //static fields for non-task animating



#if UNITY_EDITOR
        private void OnValidate()
        {
            _canvas = GetComponent<Canvas>();
            _img = GetComponentInChildren<Image>();
            _canvasRectTransfrom = GetComponent<RectTransform>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        private void Awake()
        {
            _inst = this;

        }

        private void Update()
        {
            AdjustSize();
        }


        public static void SetFocusTarget(Transform target)
        {

        }


        public static void TranslateRadius(float destRadius, float time)
        {

        }

        public static async Task TranslateRadiusAsync(float destRadius, float time)
        {

        }

        public static void TranslateRadiusWithSpeed(float destRadius, float speed)
        {

        }

        public static async Task TranslateRadiusWithSpeedAsync(float destRadius, float speed)
        {

        }


        private void AdjustSize()
        {
            var rect = _canvasRectTransfrom.rect;
            var w = rect.width;
            var h = rect.height;

            float max = System.Math.Max(w, h);

            _img.rectTransform.sizeDelta = new Vector2(max, max);
        }

    }
}