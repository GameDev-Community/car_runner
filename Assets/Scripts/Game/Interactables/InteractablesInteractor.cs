using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactables
{
    public class InteractablesInteractor : MonoBehaviour
    {
        [SerializeField] private Game.Core.Player _player;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Collider _collider;

        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private UnityEvent<Interactables.InteractableItem> _onInteract_InteractableItem;

#if UNITY_EDITOR
        [SerializeField] private bool _enableCheckSphereGizmo = true;
#endif

        private Collider[] _buffer;
        private float _checkRadius;
        private Transform _tr;

        private readonly object _collectEventLocker = new();
        private readonly object _collect_InterItemEventLocker = new();


        public event UnityAction OnInteract
        {
            add
            {
                lock (_collectEventLocker)
                {
                    _onInteract.AddListener(value);
                }
            }
            remove
            {
                lock (_collectEventLocker)
                {
                    _onInteract.RemoveListener(value);
                }
            }
        }

        public event UnityAction<Interactables.InteractableItem> OnInteract_InteractableItem
        {
            add
            {
                lock (_collect_InterItemEventLocker)
                {
                    _onInteract_InteractableItem.AddListener(value);
                }
            }
            remove
            {
                lock (_collect_InterItemEventLocker)
                {
                    _onInteract_InteractableItem.RemoveListener(value);
                }
            }
        }


        private void Awake()
        {
            _buffer = new Collider[128];
            var extends = _collider.bounds.extents;
            _checkRadius = Mathf.Max(extends.x, extends.y, extends.z);
            _tr = transform;
        }


        private void FixedUpdate()
        {
            DetectInteractables();
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.DrawWireSphere(_collider.bounds.center, _checkRadius);
        }
#endif


        private void DetectInteractables()
        {
            var p = _collider.bounds.center;
            int c = Physics.OverlapSphereNonAlloc(p, _checkRadius, _buffer, _layerMask);

            if (c == 0)
                return;

            var b = _buffer;
            var r = _tr.rotation;
            for (int i = -1; ++i < c;)
            {
                var otherCol = b[i];
                var otherTr = otherCol.transform;
                var otherP = otherTr.position;
                var otherRot = otherTr.rotation;
                if (Physics.ComputePenetration(_collider, p, r, otherCol, otherP, otherRot, out _, out _))
                {
                    if (otherCol.TryGetComponent(out Interactables.InteractableItem interactable))
                    {
                        _onInteract?.Invoke();
                        _onInteract_InteractableItem?.Invoke(interactable);
                        interactable.Interact(_player);
                    }
                }
            }
        }
    }
}
