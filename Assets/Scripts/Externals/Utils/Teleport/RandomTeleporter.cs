using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(Collider))]
    public class RandomTeleporter : MonoBehaviour
    {
        [SerializeField] private Transform[] _destinations;


        private void OnTriggerEnter(Collider other)
        {
            var t = other.gameObject.GetComponentInParent<Teleportable>();
            if (t != null)
            {
                var d = _destinations[Random.Range(0, _destinations.Length)];
                t.TeleportTo(d);
            }
        }
    }
}