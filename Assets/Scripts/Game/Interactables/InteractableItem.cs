using Externals.Utils;
using Game.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactables
{
    public class InteractableItem : MonoBehaviour
    {
        [Header("Meta")]
        [SerializeField] private MetaInfo _metaInfo;
        [Space]
        [SerializeField, Range(-1f, 1f)] private float _goodnesValue;
        [SerializeField] private Collider _collider;
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private UnityEvent<Player> _onInteract_Player;
        [SerializeField] private UnityEvent<Vector3> _onInteract_InteractorPosition;
        [Space]
        [SerializeField] private InteractableActionBase[] _interactableActions;
        [Header("Quicks")]
        [Tooltip("-1 for no autodestruction visuals")]
        [SerializeField] private float _autodestructionDelay = 0;

        private readonly object _interactEventLocker = new();
        private readonly object _interact_PlayerEventLocker = new();


        public void Init(MetaInfo metaInfo, float? goodnessV, Collider collider, UnityEvent onInteract,
            UnityEvent<Player> onInteract_Player,
            UnityEvent<Vector3> onInteract_InteractorPosition, float? autodestructionDelay)
        {
            if (metaInfo != null)
                _metaInfo = metaInfo;

            if (goodnessV.HasValue)
                _goodnesValue = goodnessV.Value;

            if (collider != null)
                _collider = collider;

            if (onInteract != null)
                _onInteract = onInteract;

            if (onInteract_Player != null)
                _onInteract_Player = onInteract_Player;

            if (onInteract_InteractorPosition != null)
                _onInteract_InteractorPosition = onInteract_InteractorPosition;

            if (autodestructionDelay.HasValue)
                _autodestructionDelay = autodestructionDelay.Value;
        }


        public MetaInfo MetaInfo => _metaInfo;

        public float GoodnesValue => _goodnesValue;
        public Collider Collider => _collider;


        public event UnityAction OnInteract
        {
            add
            {
                lock (_interactEventLocker)
                {
                    _onInteract.AddListener(value);
                }
            }
            remove
            {
                lock (_interactEventLocker)
                {
                    _onInteract.RemoveListener(value);
                }
            }
        }

        public event UnityAction<Player> OnInteract_Player
        {
            add
            {
                lock (_interact_PlayerEventLocker)
                {
                    _onInteract_Player.AddListener(value);
                }
            }
            remove
            {
                lock (_interact_PlayerEventLocker)
                {
                    _onInteract_Player.RemoveListener(value);
                }
            }
        }


        public void Interact(Player interactor)
        {
            Destroy(_collider);
            _onInteract?.Invoke();
            _onInteract_Player?.Invoke(interactor);
            _onInteract_InteractorPosition?.Invoke(interactor.transform.position);

            if (name == "Pivo")
            {

            }
            foreach (var ia in _interactableActions)
            {
                ia.Act(interactor);
            }

            HandleInteractionInherited(interactor);

            if (_autodestructionDelay >= 0)
                Destroy(gameObject, _autodestructionDelay);
        }


        protected virtual void HandleInteractionInherited(Player interactor) { }
    }
}