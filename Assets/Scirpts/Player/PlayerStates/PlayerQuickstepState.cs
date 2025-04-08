using UnityEngine;

public class PlayerQuickstepState : PlayerState
{
    private float direction;

    public PlayerQuickstepState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.ResetVelocity();

        direction = xInput == 0 ? -player.facingDir : xInput;

        player.anim.SetInteger("facingDir", player.facingDir);
        player.stats.SwitchInvincibility(false);
    }

    public override void Update()
    {
        base.Update();
        if(!player.CloseToEdge())
            rb.linearVelocity = new Vector2(player.quickstepSpeed * direction, rb.linearVelocityY);
        else
            player.ResetVelocity();
        //ignore damage

        if(trigger)
            stateMachine.ChangeState(player.idle);
    }


    public override void Exit()
    {
        base.Exit();

        player.ResetVelocity();
        player.stats.SwitchInvincibility(true);
    }
}
