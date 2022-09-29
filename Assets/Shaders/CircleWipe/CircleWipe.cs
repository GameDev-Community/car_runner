using System;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Testing.Shaders
{
    [RequireComponent(typeof(Canvas))]
    public sealed class CircleWipe : MonoBehaviour
    {
        //[SerializeField] private Transform _target;
        [SerializeField] private Material _circleWipeMat;
        [SerializeField, HideInInspector] private Canvas _canvas;
        [SerializeField, HideInInspector] private Image _img;
        [SerializeField, HideInInspector] private RectTransform _canvasRectTransfrom;
        [SerializeField, HideInInspector] private RectTransform _imgRectTransfrom;

        //private static readonly int _radiusPropID = Shader.PropertyToID("_Radius");
        //private static readonly int _smoothPropID = Shader.PropertyToID("_Smooth");
        //private static readonly int _centerPropID = Shader.PropertyToID("_Center");

        private static readonly int _settingsPropID = Shader.PropertyToID("_Settings");
        private static Camera _cam;

        private static Vector2 _tmpRect;
        private static CircleWipe _inst;


        //todo:

        //static fields for non-task animating

        private static Transform _targetTr;
        private static Vector2 _targetScreenPos;
        private static Vector2 _center;

        private static float _timeTotal;
        private static float _timeLeft;
        private static float _speed;


        private static void SetCircleData(float radius, float smooth, Vector2 center)
        {
            var m = _inst._circleWipeMat;
            //m.SetFloat(_radiusPropID, radius);
            //m.SetFloat(_smoothPropID, smooth);
            //m.SetVector(_centerPropID, center);

            m.SetVector(_settingsPropID, new Vector4(radius, smooth, center.x, center.y));
        }



#if UNITY_EDITOR
        private void OnValidate()
        {
            _canvas = GetComponent<Canvas>();
            _img = GetComponentInChildren<Image>();
            _canvasRectTransfrom = GetComponent<RectTransform>();
            _imgRectTransfrom = _img.GetComponent<RectTransform>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        private void Awake()
        {
            _inst = this;
            _cam = Camera.main;
        }

        private async void Update()
        {
            AdjustSize();

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _targetTr = null;

                CenterOnScreenPoint(Input.mousePosition);
                await TranslateRadiusEaseAsync(0, 4f);
                SetCircleData(1, 0, new(0.5f, 0.5f));
            }
        }

        private void Test1234()
        {
            float radius = 0.2f;
            float smooth = 0.03f;
            Vector2 center;
            //var screenPos = Camera.main.WorldToScreenPoint(_target.position);
            var screenPoint = Input.mousePosition;

            var canvasRect = _canvasRectTransfrom.rect;
            var width = canvasRect.width;
            var height = canvasRect.height;

            var targetCanvasPos = new Vector2
            {
                x = screenPoint.x / Screen.width * width,
                y = screenPoint.y / Screen.height * height,
            };

            float sqrV;

            if (width > height)
            {
                sqrV = width;
                targetCanvasPos.y += (width - height) / 2f;
            }
            else
            {
                sqrV = height;
                targetCanvasPos.x += (height - width) / 2f;
            }

            targetCanvasPos /= sqrV;

            center = targetCanvasPos;
            SetCircleData(radius, smooth, center);
        }



        public static void SetFocusTarget(Transform target)
        {
            _targetTr = target;
        }


        public static void TranslateRadius(float destRadius, float time)
        {

        }

        public static async Task TranslateRadiusAsync(float destRadius, float time)
        {
            var mat = _inst._circleWipeMat;
            var initialSettings = mat.GetVector(_settingsPropID);
            float initialRadius = initialSettings.x;

            for (float timeLeft = time; timeLeft > 0; timeLeft -= Time.unscaledDeltaTime)
            {
                float newRadius = Mathf.Lerp(initialRadius, destRadius, 1 - timeLeft / time);
                SetCircleData(newRadius, 0, GetCenter());
                await Task.Yield();
            }


            SetCircleData(destRadius, 0, GetCenter());
        }

        private static AnimationCurve _eeoCurve;
        public static async Task TranslateRadiusEaseAsync(float destRadius, float time)
        {
            _eeoCurve ??= AnimationCurve.EaseInOut(0, 0, 1, 1);

            await TranslateRadiusAsync(destRadius, time, _eeoCurve);
        }

        public static async Task TranslateRadiusAsync(float destRadius, float time, AnimationCurve curve)
        {
            var mat = _inst._circleWipeMat;
            var initialSettings = mat.GetVector(_settingsPropID);
            float initialRadius = initialSettings.x;

            for (float timeLeft = time; timeLeft > 0; timeLeft -= Time.unscaledDeltaTime)
            {
                float newRadius = Mathf.Lerp(initialRadius, destRadius, curve.Evaluate(1 - timeLeft / time));
                SetCircleData(newRadius, 0, GetCenter());
                await Task.Yield();
            }


            SetCircleData(destRadius, 0, GetCenter());
        }

        public static void TranslateRadiusWithSpeed(float destRadius, float speed)
        {

        }

        public static async Task TranslateRadiusWithSpeedAsync(float destRadius, float speed)
        {
            speed = System.Math.Abs(speed);

            var mat = _inst._circleWipeMat;
            var initialSettings = mat.GetVector(_settingsPropID);
            float radius = initialSettings.x;

            bool increasing = destRadius > radius;

            if (!increasing)
                speed = -speed;

            for (; ; )
            {
                float newRadius = radius += speed * Time.unscaledDeltaTime;

                bool flag = increasing ? newRadius >= destRadius : newRadius <= destRadius;

                if (flag)
                    newRadius = destRadius;

                SetCircleData(newRadius, 0, GetCenter());

                if (flag)
                    return;

                await Task.Yield();
            }
        }


        private static Vector2 GetCenter()
        {
            if (_targetTr != null)
                CenterOnWorldPoint(_targetTr.position);

            return _center;
        }


        private static void CenterOnWorldPoint(Vector3 worldPoint)
        {
            var screenPoint = _cam.WorldToScreenPoint(worldPoint);
            CenterOnScreenPoint(screenPoint);
        }

        private static void CenterOnScreenPoint(Vector2 screenPoint)
        {
            var canvasRect = _inst._canvasRectTransfrom.rect;
            var width = canvasRect.width;
            var height = canvasRect.height;

            var canvasPoint = new Vector2
            {
                x = screenPoint.x / Screen.width * width,
                y = screenPoint.y / Screen.height * height,
            };

            float sqrV;

            if (width > height)
            {
                sqrV = width;
                canvasPoint.y += (width - height) / 2f;
            }
            else
            {
                sqrV = height;
                canvasPoint.x += (height - width) / 2f;
            }

            canvasPoint /= sqrV;

            _center = canvasPoint;
        }

        private void AdjustSize()
        {
            var rectSize = _canvasRectTransfrom.rect.size;

            if (_tmpRect == rectSize)
                return;

            _tmpRect = rectSize;

            float max = System.Math.Max(rectSize.x, rectSize.y);

            _img.rectTransform.sizeDelta = new Vector2(max, max);
        }

    }
}