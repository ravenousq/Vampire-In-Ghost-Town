using UnityEngine;

public class PlayerAirborneState : PlayerState
{
    private float bufferTimer;

    public PlayerAirborneState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        bufferTimer = 0;

        if(player.allowCoyote)
        {
            player.allowCoyote = false;
            stateTimer = player.coyoteJumpWindow;
        }
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer >= 0 && Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jump);

        bufferTimer -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && bufferTimer < 0)
            bufferTimer = player.bufferJumpWindow;

        if(xInput != 0 && rb.gravityScale > 0)
            player.SetVelocity(player.movementSpeed * .8f * xInput, rb.linearVelocityY);

        if(player.IsGroundDetected() && !player.isBusy)
            stateMachine.ChangeState(player.idle);

        if(player.IsWallDetected() && rb.linearVelocityY < 0 && player.canWallSlide)
            stateMachine.ChangeState(player.wallSlide);

        if(player.ladderToClimb && yInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.climb);
    }

    public override void Exit()
    {
        base.Exit();

        if(bufferTimer > 0)
            player.executeBuffer = true;
    }
}
