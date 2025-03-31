
using System.Collections;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public Vector2 finalDirections;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashDuration;
            
        finalDirections = Vector2.zero;
        float xDirection, yDirection;

        GetDashInput(out xDirection, out yDirection);

        finalDirections = new Vector2(xDirection * player.dashSpeed, yDirection * player.dashSpeed * .8f);
    }

    public override void Update()
    {
        base.Update();

        if(!player.isKnocked)
            player.SetVelocity(finalDirections);

        if(stateTimer < 0)
            stateMachine.ChangeState(player.airborne);
    }

    public override void Exit()
    {
        base.Exit();

        player.creatingAfterImage = false;
        player.skills.dash.SwitchBlockade(true);

        if(!player.isKnocked)
        {
            player.ResetVelocity();
            player.ZeroGravityFor(.1f);
        }
    }

    
    private void GetDashInput(out float xDirection, out float yDirection)
    {
        xDirection = xInput;
        yDirection = yInput;

        if(yInput <= 0)
            yDirection = 0;

        if(xInput == 0)
            xDirection = player.facingDir;
        
        if (xInput == 0 && yInput == 1 && !player.IsWallDetected())
        {
            xDirection = 0;
            yDirection = yInput;
        }


        if (player.IsWallDetected() && !player.IsGroundDetected())
            xDirection *= -1;
    }
}
