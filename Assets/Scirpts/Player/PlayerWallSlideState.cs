
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    private bool flipTrigger;

    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.wallSlideTime;
        flipTrigger = false;
    }

    public override void Update()
    {
        base.Update();

        float slideSpeed = yInput == -1 ? rb.linearVelocityY : rb.linearVelocityY * .7f;

        player.SetVelocity(0, slideSpeed);

        if(Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.wallJump);
        

        if(xInput == player.facingDir *- 1 && stateTimer < player.wallSlideTime - .5f)
            stateMachine.ChangeState(player.airborne);

        if(stateTimer <= 0)
        {
            flipTrigger = true;
            stateMachine.ChangeState(player.airborne);
            player.canWallSlide = false;
        }

        if(player.IsGroundDetected() || !player.IsWallDetected())
        {
            flipTrigger = true;
            stateMachine.ChangeState(player.idle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        if(flipTrigger)
            player.Flip();
    }
}
