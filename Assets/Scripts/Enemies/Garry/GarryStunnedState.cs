

using System.Diagnostics;

public class GarryStunnedState : GarryGroundedState
{
    public GarryStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Garry enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stats.stunTime;
        enemy.AllowExecution(true);
    }

    public override void Update()
    {
        base.Update();

        enemy.ResetVelocity();

        if(stateTimer < 0)
            enemy.stats.Recover();
        
    }

    public override void Exit()
    {
        base.Exit();

        enemy.AllowExecution(false);
    }
}
