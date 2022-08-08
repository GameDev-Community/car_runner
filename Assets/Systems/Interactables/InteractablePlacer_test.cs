using UnityEngine;

namespace Game.Interactables
{
    public class InteractablePlacer_test : MonoBehaviour
    {
        [SerializeField] private InteractableBase _interactable;


        private void Start()
        {
            var x = Instantiate(_interactable.Prefab, transform.position, Quaternion.identity, null);
            x.Init(_interactable);
            Destroy(gameObject);
        }
    }

}
