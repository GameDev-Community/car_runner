using System.Collections.Generic;
using UnityEngine;
using System;

public enum DamageType { FlatTires, Impact }
public class DamageStateMachine : MonoBehaviour
{
    private Dictionary<DamageType, DamageState> damageStates = new Dictionary<DamageType, DamageState>();
    private List<DamageState> currentDamageStates = new List<DamageState>();

    private void Awake()
    {
        damageStates.Add(DamageType.FlatTires, new FlatTiresDamageState());
        damageStates.Add(DamageType.Impact, new ImpactDamageState());
    }

    public void AddNewDamageState(DamageType damageType)
    {
        DamageState damageState = damageStates[damageType];
        if (currentDamageStates.Contains(damageState))
            return;
        damageState.Enter();
        currentDamageStates.Add(damageState);
    }

    public void RemoveDamageState(DamageType damageType)
    {
        DamageState damageState = damageStates[damageType];
        if (currentDamageStates.Contains(damageState))
        {
            damageState.Exit();
            currentDamageStates.Remove(damageState);
        }
        else
        {
            throw new ArgumentException($"Damage type {damageType} wasn't found in currentDamageStates");
        }
    }
}
