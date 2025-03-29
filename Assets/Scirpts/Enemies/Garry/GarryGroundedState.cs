using UnityEngine;

public class GarryGroundedState : EnemyState
{
    protected Garry enemy;

    private Event gotDamaged;

    public GarryGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Garry enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.onDamaged += enemy.BecomeAggresive;
    }

    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.aggro);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
