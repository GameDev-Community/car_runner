using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(Collider))]
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private Transform _destination;


        private void OnTriggerEnter(Collider other)
        {
            var t = other.gameObject.GetComponentInParent<Teleportable>();
            if (t != null)
                t.TeleportTo(_destination);
        }
    }
}