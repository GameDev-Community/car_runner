using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Singleton<Car>
{
    [SerializeField] private DamageStateMachine damageStateMachine;

    [SerializeField] private float speed;

    public void GetDamage(bool isIndestructible, DamageType damageType)
    {
        if (isIndestructible)
        {
            Dead();
            return;
        }

        damageStateMachine.AddNewDamageState(damageType);
    }

    public void Dead()
    {

    }
}
