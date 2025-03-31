using UnityEngine;

public class GarryIdleState : GarryGroundedState
{
    public GarryIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Garry enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        enemy.ResetVelocity();

        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.move);

        if(enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.aggro);
    }

    public override void Exit()
    {
        base.Exit();

        if(!enemy.patrolRoute && (!enemy.IsGroundDetected() || enemy.IsWallDetected()))
            enemy.Flip();

    }
}
