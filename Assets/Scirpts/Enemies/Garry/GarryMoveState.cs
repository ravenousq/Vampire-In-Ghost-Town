
using UnityEditor.Tilemaps;
using UnityEngine;

public class GarryMoveState : GarryGroundedState
{
    public GarryMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Garry enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 2f;

        if(enemy.patrolRoute && !enemy.IsWallOnTheBackDetected())
            enemy.Flip();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.movementSpeed * enemy.facingDir, rb.linearVelocityY);

        if(enemy.patrolRoute)
            if(Vector2.Distance(enemy.transform.position, enemy.patrolRoute.transform.position) > enemy.patrolRoute.bounds.size.x /2 && stateTimer < 0)
                stateMachine.ChangeState(enemy.idle);  

        if(!enemy.IsGroundDetected() || enemy.IsWallDetected())
            stateMachine.ChangeState(enemy.idle);
    
        if(enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.aggro);
    }

    public override void Exit()
    {
        base.Exit();

        if(enemy.IsWallDetected()|| !enemy.IsGroundDetected())
            enemy.Flip();
    }
}
