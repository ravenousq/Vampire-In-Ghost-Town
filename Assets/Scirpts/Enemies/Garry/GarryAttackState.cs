using UnityEngine;

public class GarryAttackState : GarryGroundedState
{
    public GarryAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Garry enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(trigger)
            stateMachine.ChangeState(enemy.aggro);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
