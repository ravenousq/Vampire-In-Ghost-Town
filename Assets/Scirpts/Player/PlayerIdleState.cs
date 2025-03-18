using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
           
        player.ResetVelocity();
    }

    public override void Update()
    {
        base.Update();

        if(xInput != 0 && !player.executeBuffer)
            player.stateMachine.ChangeState(player.move);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
