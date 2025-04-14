using System;
using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    
    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    public String animBoolName;

    protected float stateTimer;
    protected bool trigger;
    protected SkillManager skills;

    public PlayerState(Player player, PlayerStateMachine stateMachine, String animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        trigger = false;
        skills = SkillManager.instance;

        AxisInput();
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        AxisInput();
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);

        AxisInput();
    }

    protected virtual void AxisInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.anim.SetFloat("yVelocity", rb.linearVelocityY);
        player.anim.SetFloat("xVelocity", rb.linearVelocityX);
    }

    public virtual void CallTrigger() => trigger = true;
}
