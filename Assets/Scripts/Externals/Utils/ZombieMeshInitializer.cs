using UnityEngine;

namespace Utils
{
    public class ZombieMeshInitializer : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer[] zombieMeshes;

        private void Start()
        {
            zombieMeshes[Random.Range(0, zombieMeshes.Length)].gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (zombieMeshes.Length > 0)
                Gizmos.DrawCube(zombieMeshes[0].bounds.center, zombieMeshes[0].bounds.size);
        }
#endif
    }
}
