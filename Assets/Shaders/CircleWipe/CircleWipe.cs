using System;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor.Build.Content;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Testing.Shaders
{

    [RequireComponent(typeof(Canvas))] //to aquire rect transform
    public sealed class CircleWipe : MonoBehaviour
    {
        //[SerializeField] private Transform _target;
        [SerializeField] private Material _circleWipeMat;
        [SerializeField] private AnimationCurve _defaultCurve;
        [SerializeField] private AnimationCurve _defaultClosingCurve;
        [SerializeField] private AnimationCurve _defaultOpeningCurve;
        //[SerializeField, HideInInspector] private Canvas _canvas;
        [SerializeField, HideInInspector] private Image _img;
        [SerializeField, HideInInspector] private RectTransform _canvasRectTransfrom;
        //[SerializeField, HideInInspector] private RectTransform _imgRectTransfrom;

        //private static readonly int _radiusPropID = Shader.PropertyToID("_Radius");
        //private static readonly int _smoothPropID = Shader.PropertyToID("_Smooth");
        //private static readonly int _centerPropID = Shader.PropertyToID("_Center");

        private static readonly int _settingsPropID = Shader.PropertyToID("_Settings");
        private static readonly Vector4 _defaultSettings = new(1, 0, 0.5f, 0.5f);
        private static Camera _cam;
        private static bool _enabled;

        private static Vector2 _tmpRect;
        private static CircleWipe _inst;


        //todo:

        //static fields for non-task animating

        private static Transform _targetTr;
        private static Vector2 _targetScreenPos;
        private static Vector2 _center;

        //private static float _timeTotal;
        //private static float _timeLeft;
        //private static float _speed;


        public static bool Enabled
        {
            get => _enabled;

            set
            {
                if (value == _enabled)
                    return;

                _inst.enabled = _enabled = value;
            }
        }

        public static float MaterialRadius
        {
            get
            {
                var settings = _inst._circleWipeMat.GetVector(_settingsPropID);
                return settings.x;
            }
        }

        public static float MaterialSmoothness
        {
            get
            {
                var settings = _inst._circleWipeMat.GetVector(_settingsPropID);
                return settings.y;
            }
        }

        public static Vector2 MaterialCenter
        {
            get
            {
                var settings = _inst._circleWipeMat.GetVector(_settingsPropID);
                return new Vector2(settings.z, settings.w);
            }
        }

        public static Color MaterialColor
        {
            get => _inst._circleWipeMat.color;
            set => _inst._circleWipeMat.color = value;
        }


        public static float TargetRadius { get; set; }
        public static float TargetSmoothness { get; set; }


        public static float ClosedRadius { get; set; } = 0f;
        public static float OpenedRadius { get; set; } = 1f;


#if UNITY_EDITOR
        private void OnValidate()
        {
            //_canvas = GetComponent<Canvas>();
            _img = GetComponentInChildren<Image>();
            _canvasRectTransfrom = GetComponent<RectTransform>();
            //_imgRectTransfrom = _img.GetComponent<RectTransform>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        private void Awake()
        {
            
            if (_inst != this)
            {
                if (_inst != null)
                {
                    Destroy(gameObject);
                    return;
                }

                _inst = this;
            }

            DontDestroyOnLoad(gameObject);

            _enabled = enabled;
            SetCircleData(_defaultSettings);
            _center = new Vector2(_defaultSettings.z, _defaultSettings.w);


            _cam = Camera.main;

            Enabled = false;
        }


        private void Update()
        {
            AdjustSize();
        }


        public static void SetCircleData(float radius, float smooth, Vector2 center)
        {
            var m = _inst._circleWipeMat;
            //m.SetFloat(_radiusPropID, radius);
            //m.SetFloat(_smoothPropID, smooth);
            //m.SetVector(_centerPropID, center);

            m.SetVector(_settingsPropID, new Vector4(radius, smooth, center.x, center.y));
        }

        public static void SetCircleData(Vector4 data)
        {
            var m = _inst._circleWipeMat;
            m.SetVector(_settingsPropID, data);
        }

        //private async void Update()
        //{
        //    AdjustSize();

        //    if (Input.GetKeyDown(KeyCode.Mouse0))
        //    {
        //        _targetTr = null;

        //        CenterOnScreenPoint(Input.mousePosition);
        //        await TranslateRadiusDefaultCurveAsync(0, 1.2f);
        //        SetCircleData(1, 0, new(0.5f, 0.5f));
        //    }
        //}

        //private void Test1234()
        //{
        //    float radius = 0.2f;
        //    float smooth = 0.03f;
        //    Vector2 center;
        //    //var screenPos = Camera.main.WorldToScreenPoint(_target.position);
        //    var screenPoint = Input.mousePosition;

        //    var canvasRect = _canvasRectTransfrom.rect;
        //    var width = canvasRect.width;
        //    var height = canvasRect.height;

        //    var targetCanvasPos = new Vector2
        //    {
        //        x = screenPoint.x / Screen.width * width,
        //        y = screenPoint.y / Screen.height * height,
        //    };

        //    float sqrV;

        //    if (width > height)
        //    {
        //        sqrV = width;
        //        targetCanvasPos.y += (width - height) / 2f;
        //    }
        //    else
        //    {
        //        sqrV = height;
        //        targetCanvasPos.x += (height - width) / 2f;
        //    }

        //    targetCanvasPos /= sqrV;

        //    center = targetCanvasPos;
        //    SetCircleData(radius, smooth, center);
        //}



        public static void SetFocusTarget(Transform target)
        {
            _targetTr = target;
        }


        //public static void TranslateRadius(float destRadius, float time)
        //{

        //}

        public static async Task TranslateRadiusAsync(float destRadius, float time)
        {
            Enabled = true;
            var mat = _inst._circleWipeMat;
            var initialSettings = mat.GetVector(_settingsPropID);
            float initialRadius = initialSettings.x;

            for (float timeLeft = time; timeLeft > 0; timeLeft -= Time.unscaledDeltaTime)
            {
                float newRadius = Mathf.Lerp(initialRadius, destRadius, 1 - timeLeft / time);
                SetCircleData(newRadius, TargetSmoothness, GetCenter());
                await Task.Yield();
            }


            SetCircleData(destRadius, 0, GetCenter());
        }

        public static async Task CloseAsync(float time)
        {
            await TranslateRadiusAsync(ClosedRadius, time);
        }

        public static async Task CloseAsync(float time, AnimationCurve curve)
        {
            await TranslateRadiusAsync(ClosedRadius, time, curve);
        }

        public static async Task CloseWithDefaultCurveAsync(float time)
        {
            await CloseAsync(time, _inst._defaultClosingCurve);
        }

        public static async Task OpenAsync(float time)
        {
            await TranslateRadiusAsync(OpenedRadius, time);
            Enabled = false;
        }

        public static async Task OpenAsync(float time, AnimationCurve curve)
        {
            await TranslateRadiusAsync(OpenedRadius, time, curve);
            Enabled = false;
        }

        public static async Task OpenWithDefaultCurveAsync(float time)
        {
            await OpenAsync(time, _inst._defaultOpeningCurve);
        }

        public static async Task TranslateRadiusDefaultCurveAsync(float destRadius, float time)
        {
            await TranslateRadiusAsync(destRadius, time, _inst._defaultCurve);
        }

        public static async Task TranslateRadiusAsync(float destRadius, float time, AnimationCurve curve)
        {
            Enabled = true;
            float initialRadius = MaterialRadius;

            for (float timeLeft = time; timeLeft > 0; timeLeft -= Time.unscaledDeltaTime)
            {
                float newRadius = Mathf.Lerp(initialRadius, destRadius, curve.Evaluate(1 - timeLeft / time));
                UnityEngine.Debug.Log(newRadius);
                SetCircleData(newRadius, 0, GetCenter());
                await Task.Yield();
            }


            SetCircleData(destRadius, TargetSmoothness, GetCenter());
        }

        //public static void TranslateRadiusWithSpeed(float destRadius, float speed)
        //{

        //}

        public static async Task TranslateRadiusWithSpeedAsync(float destRadius, float speed)
        {
            Enabled = true;
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

                SetCircleData(newRadius, TargetSmoothness, GetCenter());

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

        private static void AdjustSize()
        {
            var rectSize = _inst._canvasRectTransfrom.rect.size;

            if (_tmpRect == rectSize)
                return;

            _tmpRect = rectSize;

            float max = System.Math.Max(rectSize.x, rectSize.y);

            _inst._img.rectTransform.sizeDelta = new Vector2(max, max);
        }

    }
}