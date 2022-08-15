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
        [SerializeField] private UnityEvent<GameObject> _onInteract_GameObject;
        [Tooltip("negative for no selfdestruction")]
        [SerializeField] private float _selfDestructionCooldown = 0;

        private InteractableBase _source;


        public Collider Collider => _collider;


        public void Init(InteractableBase source)
        {
            _source = source;
        }


        public void Interact(Player player)
        {
            Destroy(_collider);
            _onInteract?.Invoke();
            _onInteract_Vector3?.Invoke(player.transform.position);
            _onInteract_GameObject?.Invoke(player.gameObject);
            _source.Interact(player);

            if (_selfDestructionCooldown >= 0)
                Destroy(gameObject, _selfDestructionCooldown);
        }
    }

}
