using UnityEngine;

namespace Utils
{
    public class WreckableItem : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] _wreckages;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _explosionRadius;

        public virtual void Wreck(Vector3 explosionPosition)
        {
            foreach (var wreckage in _wreckages)
            {
                wreckage.isKinematic = false;
                wreckage.AddExplosionForce(_explosionForce, explosionPosition, _explosionRadius, 0f, ForceMode.Impulse);
            }
        }

        public virtual void Wreck()
        {
            Wreck(transform.position);
        }
    }
}
