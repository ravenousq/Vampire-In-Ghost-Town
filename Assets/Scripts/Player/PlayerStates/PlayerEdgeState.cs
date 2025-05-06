using System.Threading;
using UnityEngine;

public class PlayerEdgeState : PlayerState
{
    private float defaultGravityScale;
    private bool climbing;
    float xTarget;

    public PlayerEdgeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 1f;
        climbing = false;
        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        xTarget = player.transform.position.x + player.cd.size.x * player.facingDir;
        skills.ChangeLockOnAllSkills(true);
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();

        if(stateTimer < 0 && !climbing && Input.GetKeyDown(KeyCode.S))
            stateMachine.ChangeState(player.airborne);

        if(stateTimer < 0 && !climbing && Input.GetKeyDown(KeyCode.W) || Input.GetAxisRaw("Horizontal") == player.facingDir)
            climbing = true;

        if(climbing)
        {

            if(Physics2D.Raycast(player.groundCheck.position, Vector2.right * player.facingDir, player.cd.size.x/2, player.whatIsGround))
                player.transform.position = Vector2.MoveTowards(player.transform.position, player.transform.position + Vector3.up, 4 * Time.deltaTime);
            else
                player.transform.position = Vector2.MoveTowards(player.transform.position, new Vector3(xTarget, player.transform.position.y), 4 * Time.deltaTime);
        }

        if(player.transform.position.x == xTarget)
            stateMachine.ChangeState(player.idle);

    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = defaultGravityScale;
        player.BusyFor(.4f);
        skills.ChangeLockOnAllSkills(false);
    }
}
