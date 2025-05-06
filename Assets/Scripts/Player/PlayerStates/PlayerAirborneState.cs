
using UnityEngine;

public class PlayerAirborneState : PlayerState
{
    private float bufferTimer;
    public Vector2 edge { get; private set; } = Vector2.zero;

    public PlayerAirborneState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        bufferTimer = 0;

        if(player.allowCoyote)
        {
            player.allowCoyote = false;
            stateTimer = player.coyoteJumpWindow;
        }
    }

    public override void Update()
    {
        base.Update();
        
        CheckForEdge();

        if (edge != Vector2.zero & !player.isBusy && Vector2.Distance(player.edgeCheck.position, edge) < .5f)
            stateMachine.ChangeState(player.edge);

        if (stateTimer >= 0 && Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jump);

        bufferTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && bufferTimer < 0)
            bufferTimer = player.bufferJumpWindow;

        if (xInput != 0 && rb.gravityScale > 0 && !player.isKnocked)
            player.SetVelocity(player.movementSpeed * .8f * xInput, rb.linearVelocityY);

        if (player.IsGroundDetected() && !player.isBusy)
        {
            player.InstantiateFX(player.landFX, player.groundCheck, new Vector3(0, .8f), Vector3.zero);
            stateMachine.ChangeState(player.idle);
        }

        if (player.IsWallDetected() && rb.linearVelocityY < 0 && player.canWallSlide)
            stateMachine.ChangeState(player.wallSlide);

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Mouse0) && SkillManager.instance.isSkillUnlocked("Into The Abyss"))
            stateMachine.ChangeState(player.dive);
    }

    private void CheckForEdge()
    {
        RaycastHit2D possiblyEdge = Physics2D.Raycast(player.edgeCheck.position, Vector2.down, player.cd.size.y / 2, player.whatIsGround);

        if (possiblyEdge && possiblyEdge.point.y < player.edgeCheck.position.y)
        {
            Vector2 edgeIntersection = possiblyEdge.point;

            float horizontalOffset = player.cd.size.x / 2 + 0.05f;
            Vector2 horizontalRayOrigin = new Vector2(player.transform.position.x + player.facingDir * horizontalOffset, edgeIntersection.y - 0.1f);

            RaycastHit2D finalEdgeChecker = Physics2D.Raycast(horizontalRayOrigin, Vector2.right * player.facingDir, 0.1f, player.whatIsGround);

            if (finalEdgeChecker)
                edge = finalEdgeChecker.point;
        }
    }

    public override void Exit()
    {
        base.Exit();

        if(bufferTimer > 0)
            player.executeBuffer = true;

        edge = Vector2.zero;
    }
}
