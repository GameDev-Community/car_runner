using UnityEngine;

public class FlatTiresDamageState : DamageState
{
    public override void Enter()
    {
        Debug.Log("Enter in flat tires damage state");
    }

    public override void Exit()
    {
        Debug.Log("Exit from flat tires damage state");
    }
}
