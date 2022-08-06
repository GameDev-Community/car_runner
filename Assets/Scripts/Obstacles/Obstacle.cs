using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { FlatTires, Impact, LockedWheels }
public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool isIndestructible = false;
    [SerializeField] private DamageType damageType;

    public DamageType DamageType { get => damageType; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Car.Instance.GetDamage(isIndestructible, damageType);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // animations, effects
    }
}
