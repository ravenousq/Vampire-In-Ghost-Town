
using System.Collections;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private Vector2 finalDirections;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        finalDirections = Vector2.zero;
        float xDirection, yDirection;

        GetDashInput(out xDirection, out yDirection);

        finalDirections = new Vector2(xDirection * player.dashSpeed, yDirection * player.dashSpeed * .8f);

        stateTimer = player.dashDuration;

        
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(finalDirections);

        if(stateTimer < 0)
            stateMachine.ChangeState(player.airborne);
    }

    public override void Exit()
    {
        base.Exit();

        player.creatingAfterImage = false;
        player.canDash = false;
        player.ResetVelocity();
        player.PostDash();
    }

    
    private void GetDashInput(out float xDirection, out float yDirection)
    {
        if (xInput == 0 && yInput == 0)
        {
            xDirection = player.facingDir;
            yDirection = 0;
        }
        else if (xInput == 0 && yInput != 0)
        {
            xDirection = 0;
            yDirection = yInput;
        }
        else
        {
            xDirection = player.facingDir;
            yDirection = yInput;
        }

        if (player.IsWallDetected() && !player.IsGroundDetected())
            xDirection *= -1;

        if (yDirection < 0)
        {
            yDirection = 0;
            xDirection = player.facingDir;
        }
    }
}
