using UnityEngine;

public class PlayerQuickstepState : PlayerGroundedState
{
    public PlayerQuickstepState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        int direction = xInput == 0 ? -player.facingDir : Mathf.RoundToInt(xInput) ;

        rb.linearVelocity = new Vector2(player.quickstepSpeed * direction, rb.linearVelocityY);
    }

    public override void Update()
    {
        base.Update();

        //ignore damage

        if(trigger)
            stateMachine.ChangeState(player.idle);
    }


    public override void Exit()
    {
        base.Exit();

        player.ResetVelocity();
    }
}
