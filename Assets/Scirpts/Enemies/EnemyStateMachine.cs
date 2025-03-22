using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState current { get; private set; }

    public void Initialize(EnemyState startState)
    {
        current = startState;
        current.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        current.Exit();
        current = newState;
        current.Enter();
    }
}
