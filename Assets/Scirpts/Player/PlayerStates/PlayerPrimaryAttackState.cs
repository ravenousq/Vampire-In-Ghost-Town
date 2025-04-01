using System;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float attackDir;

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        attackDir = player.facingDir;

        if(xInput != 0)
            attackDir = xInput;

        player.FlipController(attackDir);

        if(lastTimeAttacked < Time.time - player.attackWindow || comboCounter > 2)
            comboCounter = 0;

        stateTimer = .1f;

        player.anim.SetInteger("combo", comboCounter);

        int bullets = comboCounter > 1 ? -2 : -1;

        player.ModifyBullets(bullets);
    }

    public override void Update()
    {
        base.Update();


        if(stateTimer < 0 || player.CloseToEdge())
            player.ResetVelocity();
        else
            rb.linearVelocityX = player.attackMovement[comboCounter] * attackDir;

        if(player.attackTrigger)
        {
            DamageTargets();
            player.attackTrigger = false;
        }

        if(trigger)
            stateMachine.ChangeState(player.idle);
        
    }

    public override void Exit()
    {
        base.Exit();

        player.BusyFor(.15f);
        lastTimeAttacked = Time.time;
        comboCounter++;

        if(comboCounter == 2)
            player.ThirdAttack();
    }

    private void DamageTargets()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(player.transform.position.x + player.cd.size.x / 1.5f * player.facingDir, player.transform.position.y + player.cd.size.y / 3), Vector2.right * player.facingDir);

        float damageDecrease = 1;

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                break;

            else if(hit.collider.gameObject.GetComponent<Enemy>())
            {
                player.stats.DoDamage(hit.collider.gameObject.GetComponent<EnemyStats>(), Vector2.zero, 0, player.poiseDamage, damageDecrease);

                if(comboCounter == 2)
                    player.stats.DoDamage(hit.collider.gameObject.GetComponent<EnemyStats>(), Vector2.zero, 0, player.poiseDamage, damageDecrease);
                    
                damageDecrease *= .7f;
            }

            if(!player.skills.isSkillUnlocked("Vokul Fen Mah"))
                return;
        }
    }
}
