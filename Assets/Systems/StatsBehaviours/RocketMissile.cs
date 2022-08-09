using UnityEngine;
using UnityEngine.Events;

namespace Game.StatsBehaviours
{
    public class RocketMissile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _angularSpeed;
        [SerializeField] private float _impactDist = 1f;
        [SerializeField] private UnityEvent _onImpact;
        [SerializeField] private float _destroyOnImpact = -1;

        private Transform _target;
        private Vector3? _dotTarget;
        private float _impactDistSqr;


        public float Speed { get => _speed; set => _speed = value; }


        private void Awake()
        {
            _impactDistSqr = _impactDist * _impactDist;
        }

        public void Init(Transform target)
        {
            _target = target;
        }

        public void Init(Vector3 dotTarget)
        {
            _dotTarget = dotTarget;
        }


        private void Update()
        {
            if (!_dotTarget.HasValue && _target == null)
            {
                Impact();
                return;
            }

            var tr = transform;
            var trP = tr.position;
            var targetP = _target == null ? _dotTarget.Value : _target.position;
            var dstV = targetP - trP;

            if ((dstV).sqrMagnitude <= _impactDistSqr)
            {
                Impact();
                return;
            }

            trP = tr.position = Vector3.MoveTowards(trP, targetP,
                _speed * Time.deltaTime);

            var r = Quaternion.RotateTowards(tr.rotation,
                Quaternion.LookRotation(targetP - trP),
                _angularSpeed * Time.deltaTime);

            tr.rotation = r;
        }

        private void Impact()
        {
            _onImpact?.Invoke();

            if (_destroyOnImpact >= 0)
            {
                Destroy(gameObject, _destroyOnImpact);
            }
        }
    }
}
