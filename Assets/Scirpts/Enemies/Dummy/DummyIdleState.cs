using UnityEngine;

public class DummyIdleState : EnemyState
{
    protected Dummy dummy;

    public DummyIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dummy dummy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.dummy = dummy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
