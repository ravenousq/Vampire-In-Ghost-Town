using UnityEngine;

public class PlayerClimbState : PlayerState
{
    private int sideToExit;

    public PlayerClimbState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.skills.dash.SwitchBlockade(true);
        player.skills.halo.SwitchBlockade(true);
        
        stateTimer = 1f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        sideToExit = player.ladderToClimb.GetComponent<Ladder>().sideToExit;

        player.stats.OnDamaged += FallOfTheLadder;
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();

        player.anim.speed = yInput == 0 ? 0 : 1;

        if(player.ladderToClimb && player.transform.position.x != player.ladderToClimb.transform.position.x)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, new Vector3(player.ladderToClimb.transform.position.x, player.transform.position.y), 13 * Time.deltaTime);
            return;
        }
            
        if(Input.GetKey(KeyCode.LeftShift) && yInput == -1)
        {
            player.anim.SetBool("ladderSlide", true);
            player.transform.position += new Vector3(0, yInput * player.climbSpeed * 2 * Time.deltaTime, 0);
        }
        else
        {
            player.anim.SetBool("ladderSlide", false);    
            player.transform.position += new Vector3(0, yInput * player.climbSpeed * Time.deltaTime, 0);
        }
        

        if(player.IsGroundDetected() && yInput == -1)
            stateMachine.ChangeState(player.idle);

        if(!player.ladderToClimb)
            stateMachine.ChangeState(player.airborne);
    }

    public override void Exit()
    {
        base.Exit();

        player.skills.dash.SwitchBlockade(false);
        player.skills.halo.SwitchBlockade(false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        sideToExit = sideToExit == 0 ? player.facingDir : sideToExit;
        player.SetVelocity(3 * sideToExit, 3);
        player.BusyFor(.4f);
        player.anim.speed = 1;
        player.stats.OnDamaged -= FallOfTheLadder;
    }

    private void FallOfTheLadder() => player.TriggerLadder(null);
}
