using UnityEngine;

namespace Utils
{
    public class ZombieMeshInitializer : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer[] zombieMeshes;
        [SerializeField] private bool debugMode = false;

        private void Start()
        {
            zombieMeshes[Random.Range(0, zombieMeshes.Length)].gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (debugMode && zombieMeshes.Length > 0)
                Gizmos.DrawCube(zombieMeshes[0].bounds.center, zombieMeshes[0].bounds.size);
        }
#endif
    }
}
