using System.Linq.Expressions;
using System.Threading;
using UnityEngine;

public class PlayerDiveState : PlayerState
{
    public PlayerDiveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.skills.dash.SwitchBlockade(true);
        player.skills.halo.SwitchBlockade(true);
        
        player.ZeroGravityFor(.5f);
        stateTimer = .6f;
    }

    public override void Update()
    {
        base.Update();

        if(rb.gravityScale != 0)
            rb.linearVelocity = new Vector2(0, Mathf.Lerp(0, -player.diveSpeed, 1f));
        else
            rb.linearVelocity = new Vector2(0, 1);

        if(stateTimer < 0 && Physics2D.Raycast(player.transform.position - new Vector3(player.cd.size.x /2, 0), Vector2.down, 4.5f, player.whatIsGround))
        {
            if(Input.GetKeyDown(KeyCode.Q) && SkillManager.instance.isSkillUnlocked("Closer To The Sun"))
            {
                player.floorParry = true;
                player.skills.dash.SwitchBlockade(false);
                stateMachine.ChangeState(player.jump);
            }
        }

        if(player.IsGroundDetected())
        {
            DamageImpact();

            stateMachine.ChangeState(player.idle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.skills.dash.SwitchBlockade(false);
        player.skills.halo.SwitchBlockade(false);
    }

    private void DamageImpact()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(player.transform.position, 5);

        foreach(var target in targets)
        {
            target.GetComponent<Enemy>()?.Damage();
            target.GetComponent<Enemy>()?.Knockback(new Vector2(2, 2), player.transform.position.x, .5f);
        }
    }
}
