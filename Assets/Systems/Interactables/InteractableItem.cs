using Game.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactables
{
    public class InteractableItem : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private UnityEvent _onInteract;
        [SerializeField] private UnityEvent<Vector3> _onInteract_Vector3;
        [Tooltip("negative for no selfdestruction")]
        [SerializeField] private float _selfDestructionCooldown = -1;

        private InteractableBase _source;


        public Collider Collider => _collider;


        public void Init(InteractableBase source)
        {
            _source = source;
        }

        public void Interact(Racer interactor)
        {
            Destroy(_collider);
            _onInteract?.Invoke();
            _onInteract_Vector3?.Invoke(interactor.transform.position);
            _source.Interact(interactor);
            if (_selfDestructionCooldown >= 0)
                Destroy(gameObject, _selfDestructionCooldown);
        }
    }

}
