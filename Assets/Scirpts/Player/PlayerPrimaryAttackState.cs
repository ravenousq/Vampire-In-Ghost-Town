using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private List<Collider2D> targets = new List<Collider2D>();

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        float attackDir = player.facingDir;

        if(xInput != 0)
            attackDir = xInput;

        player.FlipController(attackDir);

        if(lastTimeAttacked < Time.time - player.attackWindow || comboCounter > 2)
            comboCounter = 0;
        
        rb.linearVelocityX = player.attackMovement[comboCounter] * attackDir;
        stateTimer = .1f;

        player.anim.SetInteger("combo", comboCounter);

        int bullets = comboCounter > 1 ? -2 : -1;

        player.ModifyBullets(bullets);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0 || player.CloseToEdge())
            player.ResetVelocity();

        if(player.attackTrigger)
        {
            DamageTargets();
            player.attackTrigger = false;
        }

        if (trigger)
            stateMachine.ChangeState(player.idle);
        
    }

    public override void Exit()
    {
        base.Exit();

        player.BusyFor(.15f);
        lastTimeAttacked = Time.time;
        comboCounter++;
        targets.Clear();

        if(comboCounter == 2)
            player.ThirdAttack();
    }

    private void DamageTargets()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(player.transform.position.x + player.cd.size.x / 2, player.transform.position.y + player.cd.size.y / 3), Vector2.right * player.facingDir);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                break;

            if (hit.collider.gameObject.GetComponent<Enemy>())
                targets.Add(hit.collider);
        }

        foreach(var target in targets)
        {
            target.gameObject.GetComponent<Entity>().Damage();

            if(!player.voculFenMah)
                return;

            //Decrease damage by fixed ammount
        }
    }
}
