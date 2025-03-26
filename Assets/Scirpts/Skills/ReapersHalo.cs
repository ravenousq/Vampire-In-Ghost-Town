using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReapersHalo : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private Vector2 velocity;
    private Player player;
    private float returnSpeed;
    private int numberOfBounces;
    private bool isReturning;
    private float collisionTimer = .5f;
    private float spinDuration;
    private float spinTimer;
    private float spinSpeed;
    private bool isSpinning;
    private float spinDamageWindow;
    private float damageTimer;
    


    public void SetUpHalo(Vector2 velocity, Player player, float returnSpeed, int numberOfBounces, float spinDuration, float spinSpeed, float spinDamageWindow)
    {
        this.velocity = velocity;
        this.player = player;
        this.returnSpeed = returnSpeed;
        this.numberOfBounces = numberOfBounces;
        this.spinDuration = spinDuration;
        this.spinSpeed = spinSpeed;
        this.spinDamageWindow = spinDamageWindow;

        rb.linearVelocity = velocity;
        rb.gravityScale = 0;

        Invoke(nameof(DestroyMe), 7);
    }

    private void Update()
    {
        transform.right = rb.linearVelocity;

        collisionTimer -= Time.deltaTime;
        spinTimer -= Time.deltaTime;
        damageTimer -= Time.deltaTime;

        MovementLogic();

        if (numberOfBounces < 0)
            isReturning = true;

        SpinLogic();

        if(Vector2.Distance(transform.position, player.transform.position) < .3f && collisionTimer < 0)
            DestroyMe();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            CancelInvoke(nameof(DestroyMe));

            if (spinTimer < 0)
                isReturning = true;
            else
            {
                velocity = velocity.normalized * spinSpeed;

                if (damageTimer < 0)
                {
                    Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, cd.radius, player.whatIsEnemy);

                    foreach (var enemy in enemies)
                        enemy.GetComponent<Enemy>().Damage();

                    damageTimer = spinDamageWindow;
                }
            }
        }
    }

    private void DestroyMe()
    {
        SkillManager.instance.halo.AddCooldown(3);
        Destroy(gameObject);
    } 

    private void MovementLogic()
    {

        if(!isReturning)
            BounceLogic();
        else
        {
            velocity = (player.transform.position - transform.position).normalized * returnSpeed;
            if(Vector2.Distance(transform.position, player.transform.position) < .5f)
                DestroyMe();
        }

        rb.linearVelocity = velocity;
    }

    private void BounceLogic()
    {
        LayerMask whatIsGround = player.whatIsGround;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.linearVelocity.normalized, 0.5f, whatIsGround);

        if (hit.collider)
        {
            velocity = Vector2.Reflect(velocity, hit.normal);
            numberOfBounces--;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        other.GetComponent<Enemy>()?.Damage();    
    }

    public void StopHalo()
    {
        isSpinning = true;

        if(spinTimer > 0 && spinTimer < spinDuration - .3f)
        {
            isReturning = true;
            return;
        }

        if(SkillManager.instance.isSkillUnlocked("Legend Of Steel"))
            spinTimer = spinDuration;
    }
}
