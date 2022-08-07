using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        transform.position = target.position + offset;
        transform.LookAt(target);
    }

    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
