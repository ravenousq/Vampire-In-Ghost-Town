using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        player.canWallSlide = true;
        player.canDash = true;
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.R) && player.CanReload())
            stateMachine.ChangeState(player.reload);

        if((Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) || player.executeBuffer)
            stateMachine.ChangeState(player.jump);

        if(player.facingDir == xInput && player.IsWallDetected())
            xInput = 0;

        if(!player.IsGroundDetected())
        {
            player.allowCoyote = true;
            stateMachine.ChangeState(player.airborne);
        }

        if(yInput == 1 && player.ladderToClimb && !player.isBusy)
            stateMachine.ChangeState(player.climb);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
