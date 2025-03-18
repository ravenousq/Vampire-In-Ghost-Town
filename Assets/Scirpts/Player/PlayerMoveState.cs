using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        if(player.playStartAnim)
            player.anim.SetBool("move_start", true);
        
        base.Enter();

        player.ResetVelocity();
    }

    public override void Update()
    {
        base.Update();

        player.anim.SetBool("move_start", false);

        player.SetVelocity(xInput * player.movementSpeed, rb.linearVelocityY);

        if(xInput == 0)
            stateMachine.ChangeState(player.idle);

        if(player.facingDir == xInput && player.IsWallDetected())
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        player.playStartAnim = false;
    }
}