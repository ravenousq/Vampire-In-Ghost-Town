using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ReapersHalo : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private SpriteRenderer sr;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
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
    private bool waitForEnemy;
    private bool isOrbiting;
    private float orbitingSpeed;
    private int numberOfTurns;
    private float orbitDistance;
    private Pointer pointer;
    [SerializeField] private Pointer pointerPrefab;

    public void SetUpHalo(Vector2 velocity, Player player, float returnSpeed, int numberOfBounces, float spinDuration, float spinSpeed, float spinDamageWindow, bool isOrbiting, float orbitingSpeed, int numberOfTurns, float orbitDistance)
    {
        pointer = Instantiate(pointerPrefab, transform.position, Quaternion.identity);
        pointer.SetUp(this);

        this.velocity = velocity;
        this.player = player;
        this.returnSpeed = returnSpeed;
        this.numberOfBounces = numberOfBounces;
        this.spinDuration = spinDuration;
        this.spinSpeed = spinSpeed;
        this.spinDamageWindow = spinDamageWindow;
        this.isOrbiting = isOrbiting;
        this.orbitingSpeed = orbitingSpeed;
        this.numberOfTurns = numberOfTurns;
        this.orbitDistance = orbitDistance;

        if(isOrbiting)
            velocity = new Vector2(player.facingDir * orbitingSpeed, .1f);
        

        rb.linearVelocity = velocity;
        rb.gravityScale = 0;

        Invoke(nameof(DestroyMe), 7);
    }

    private void Update()
    {
        transform.right = rb.linearVelocity;
        CountDownTimers();

        if (!isOrbiting || isSpinning)
            MovementLogic();

        if (isOrbiting && !isSpinning)
            OrbitLogic();

        if (numberOfBounces < 0)
            isReturning = true;

        if (isSpinning)
            SpinLogic();

        if (Vector2.Distance(transform.position, player.transform.position) < .3f && collisionTimer < 0)
            DestroyMe();
    }

    private void CountDownTimers()
    {
        collisionTimer -= Time.deltaTime;
        spinTimer -= Time.deltaTime;
        damageTimer -= Time.deltaTime;
    }

    private void OrbitLogic()
    {
        if (Vector2.Distance(transform.position, player.transform.position) > orbitDistance)
        {
            rb.linearVelocity = (player.transform.position - transform.position).normalized * orbitingSpeed;
            sr.sortingOrder *= -1;
        }

        if (numberOfTurns <= 0)
            isReturning = true;
    }

    private void SpinLogic()
    {
        CancelInvoke(nameof(DestroyMe));

        if (spinTimer < 0)
            isReturning = true;
        else
        {
            if(!isOrbiting)
                velocity = rb.linearVelocity.normalized * spinSpeed;
            else
                velocity = (transform.position - player.transform.position).normalized * spinSpeed;

            if (damageTimer < 0)
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, cd.radius, player.whatIsEnemy);

                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<Enemy>().Damage();
                    //enemy.GetComponent<Enemy>()?.Knockback(new Vector2(5, 0), player.transform.position.x, spinDamageWindow);
                }

                damageTimer = spinDamageWindow;
            }
        }
        
    }

    private void DestroyMe()
    {
        if(pointer)
            Destroy(pointer.gameObject);
        if(isOrbiting && !isReturning)
            return;
            
        SkillManager.instance.halo.AddCooldown(1);
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
        //other.GetComponent<Enemy>()?.Knockback(new Vector2(1, 1), transform.position.x, .2f);

        if(waitForEnemy && other.GetComponent<Enemy>())
            isSpinning = true;

        if(collisionTimer < 0 && other.GetComponent<Player>())
            numberOfTurns--;
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(waitForEnemy && other.GetComponent<Enemy>())
            isSpinning = true;

    }

    public void StopHalo()
    {
        StartCoroutine(SpinRoutine());

        if(spinTimer > 0 && spinTimer < spinDuration - .3f)
        {
            isReturning = true;
            return;
        }

        if(SkillManager.instance.isSkillUnlocked("Legend Of Steel"))
            spinTimer = spinDuration;
    }

    private IEnumerator SpinRoutine()
    {
        waitForEnemy = true;

        yield return new WaitForSeconds(.3f);

        isSpinning = true;
        waitForEnemy = false;
    }

    private void OnBecameInvisible() 
    {
        pointer?.SwitchSpriteVisibility();
    }

    private void OnBecameVisible() 
    {
        pointer?.SwitchSpriteVisibility();    
    }
}
