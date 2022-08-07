using UnityEngine;

public class ImpactDamageState : DamageState
{
    public override void Enter()
    {
        Debug.Log("Enter in impact damage state");
    }

    public override void Exit()
    {
        Debug.Log("Exit from impact damage state");
    }
}
