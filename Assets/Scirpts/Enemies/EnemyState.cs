using System;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;
    public String animBoolName;
    protected float stateTimer;
    protected bool trigger;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, String animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        enemyBase.anim.SetBool(animBoolName, true);
        rb = enemyBase.rb;
        trigger = false;

        //Debug.Log(enemyBase.gameObject.name + " is entering " + animBoolName);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        //Debug.Log(enemyBase.gameObject.name + " is updating " + animBoolName);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);

        //Debug.Log(enemyBase.gameObject.name + " is exiting " + animBoolName);
    }

    public virtual void CallTrigger() => trigger = true;
}
