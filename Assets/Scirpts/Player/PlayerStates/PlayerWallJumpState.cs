public class PlayerWallJumpState : PlayerState
{

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .17f;

        player.SetVelocity(player.movementSpeed * player.facingDir, player.jumpForce * 1.2f);
    }

    public override void Update()
    {
        base.Update();

        if(player.IsWallDetected())
            player.Flip();

        xInput = stateTimer < 0 ? xInput : player.facingDir;
        
        player.SetVelocity(player.movementSpeed * xInput, rb.linearVelocityY);

        if(rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.airborne);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
