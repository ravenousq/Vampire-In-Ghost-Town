using UnityEngine;

public class PlayerParryState : PlayerState
{
    public PlayerParryState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.skills.parry.parryWindow;
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();

        Collider2D[] hits = Physics2D.OverlapCircleAll(player.transform.position, 3, player.whatIsEnemy);

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if(enemy.canBeStunned && (enemy.facingDir != player.facingDir || SkillManager.instance.isSkillUnlocked("Anima Mundi")))
            {
                enemy.stats.LosePoise(player.skills.parry.parryPoiseDamage);
                enemy.Parried();
            }
            
        }

        if(stateTimer < 0)
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        player.BusyFor(.2f);
    }
}
