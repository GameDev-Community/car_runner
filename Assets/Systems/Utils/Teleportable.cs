using UnityEngine;

public class Teleportable : MonoBehaviour
{
    [SerializeField] private Transform _transformToTeleport;


    public void TeleportTo(Transform dest)
    {
        _transformToTeleport.SetPositionAndRotation(dest.position, dest.rotation);
    }
}
