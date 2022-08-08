using Game.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactables
{
    public class InteractableItem : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private UnityAction _onInteract;
        [Tooltip("negative for no selfdestruction")]
        [SerializeField] private float _selfDestructionCooldown = -1;

        private InteractableBase _source;


        public void Init(InteractableBase source)
        {
            _source = source;
        }

        public void Interact(Racer interactor)
        {
            Destroy(_collider);
            _source.Interact(interactor);
            if (_selfDestructionCooldown >= 0)
                Destroy(gameObject, _selfDestructionCooldown);
        }
    }

}
