using UnityEngine;

namespace Utils
{
    public class VisualHovering : MonoBehaviour
    {
        [SerializeField] private Transform _manipulatingTransform;
        [SerializeField] private float _height = 0.4f;
        [SerializeField] private float _amplitude = 0.9f;
        [SerializeField] private float _rotSpeed = 10f;
        [SerializeField] private Vector3 _rotAxis = Vector3.up;

        [Tooltip("анимации парения и вращения не будут" +
            " синхронизированны с другими VisualHovering" +
            " объектами")]
        [SerializeField] private bool _nonSynchronyzed;

        private float _initialY;
        private float _randomK;


        private void Awake()
        {
            _initialY = _manipulatingTransform.position.y;

            if (_nonSynchronyzed)
            {
                _randomK = UnityEngine.Random.Range(0, 100f);
                _manipulatingTransform.Rotate(_rotAxis, UnityEngine.Random.Range(0, 359.99f));
            }
        }

        private void Update()
        {
            float height = (float)System.Math.Sin(UnityEngine.Time.timeSinceLevelLoadAsDouble * _amplitude + _randomK) * _height + _initialY;
            _manipulatingTransform.Rotate(_rotAxis, _rotSpeed * Time.deltaTime);
            var p = _manipulatingTransform.position;
            p.y = height;
            _manipulatingTransform.position = p;
        }
    }
}
