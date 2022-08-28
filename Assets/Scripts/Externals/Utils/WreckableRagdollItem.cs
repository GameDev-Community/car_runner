using UnityEngine;

namespace Utils
{
    public class WreckableRagdollItem : WreckableItem
    {
        [SerializeField] private Animator animator;

        public override void Wreck(Vector3 explosionPosition)
        {
            animator.enabled = false;
            base.Wreck(explosionPosition);
        }

        public override void Wreck()
        {
            animator.enabled = false;
            base.Wreck();
        }
    }
}
