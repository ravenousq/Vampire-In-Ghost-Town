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

        stateTimer = 1f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        sideToExit = player.ladderToClimb.GetComponent<Ladder>().sideToExit;
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();

        if(player.ladderToClimb && player.transform.position.x != player.ladderToClimb.transform.position.x)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, new Vector3(player.ladderToClimb.transform.position.x, player.transform.position.y), 13 * Time.deltaTime);
            return;
        }
            
        if(Input.GetKey(KeyCode.LeftShift) && yInput == -1)
            player.transform.position += new Vector3(0, yInput * player.climbSpeed * 2 * Time.deltaTime, 0);
        else
            player.transform.position += new Vector3(0, yInput * player.climbSpeed * Time.deltaTime, 0);

        if(player.IsGroundDetected() && yInput == -1)
            stateMachine.ChangeState(player.idle);

        if(!player.ladderToClimb)
            stateMachine.ChangeState(player.airborne);
    }

    public override void Exit()
    {
        base.Exit();

        rb.bodyType = RigidbodyType2D.Dynamic;
        sideToExit = sideToExit == 0 ? player.facingDir : sideToExit;
        player.SetVelocity(3 * sideToExit, 3);
        player.StartCoroutine(player.BusyFor(.4f));
    }
}
