using UnityEngine;

public class PlayerJumpState : PlayerAirborneState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.StartCoroutine(nameof(player.BusyFor), .1f);

        rb.linearVelocity = new Vector2(rb.linearVelocityX, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if(rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.airborne);
    }

    public override void Exit()
    {
        base.Exit();

        player.executeBuffer = false;
    }
}
