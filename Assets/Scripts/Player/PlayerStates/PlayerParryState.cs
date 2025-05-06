using Unity.VisualScripting;
using UnityEngine;

public class PlayerParryState : PlayerState
{
    public bool canParry;

    public PlayerParryState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.skills.parry.parryWindow;
        player.stats.OnDamaged += InterruptParry;
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();

        if(canParry)
        {
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
        }

        if(trigger)
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        player.BusyFor(.2f);
        canParry = false;
        player.stats.OnDamaged -= InterruptParry;
    }

    private void InterruptParry() => stateMachine.ChangeState(player.airborne);
}
