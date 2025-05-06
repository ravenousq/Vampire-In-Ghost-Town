using UnityEngine;

public class PlayerJumpState : PlayerAirborneState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.InstantiateFX(player.jumpFX, player.groundCheck, new Vector3(0, .8f), new Vector3(0, player.facingDir == 1 ? 0 : 180, 0));

        player.StartCoroutine(nameof(player.BusyFor), .1f);

        if(player.floorParry)
            rb.linearVelocity = new Vector2(rb.linearVelocityX, player.jumpForce * 2);
        else
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
        player.floorParry = false;
    }
}
