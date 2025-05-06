using System.Collections;
using UnityEngine;
public class Entity : MonoBehaviour
{
    #region Components
    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }
    public FX fx { get; private set; }
    #endregion

    [Header("Collision")]
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] public Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected float knockbackMultiplayer = 1;

    #region Flags
    public bool isBusy{ get; protected set; }
    public bool isKnocked { get; protected set; }
    public bool canBeKnocked { get; protected set; } = true;
    public bool canMove { get; protected set; } = true;
    public int facingDir {get; protected set; } = 1;
    public bool facingRight { get; protected set; } = true;
    #endregion

    public System.Action OnFlipped;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        fx = GetComponent<FX>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    #region Velocity
    public void SetVelocity(Vector2 velocity, bool SlowMotion = false)
    {
        if(rb.bodyType == RigidbodyType2D.Static)
            return;

        FlipController(velocity.x);
        
        velocity = SlowMotion ? velocity * Time.unscaledDeltaTime : velocity;
        
        if(canMove)
        {
            if(!SlowMotion)
                rb.linearVelocity = velocity;
            else
                rb.MovePosition(rb.position + velocity);
        }
        else
            ResetVelocity();
    }

    public void SetVelocity(float x, float y, bool SlowMotion = false)
    {
        if(rb.bodyType == RigidbodyType2D.Static)
            return;

        FlipController(x);

        Vector2 velocity = new Vector3(x, y);

        velocity = SlowMotion ? velocity * Time.unscaledDeltaTime : velocity;

        if(canMove)
        {
            if(!SlowMotion)
                rb.linearVelocity = velocity;
            else
                rb.MovePosition(rb.position + velocity);
        }
        else
            ResetVelocity();
    }
    

    public void ResetVelocity()
    {
        if(rb.bodyType != RigidbodyType2D.Static)
            rb.linearVelocity = isKnocked ? rb.linearVelocity : Vector2.zero;
    }
    #endregion

    #region Coroutines
    public void InvokeName(string name, float time) => Invoke(name, time);

    public void ZeroGravityFor(float seconds)
    {
        if(rb.gravityScale == 0)
            return;

        StartCoroutine(ZeroGravityRoutine(seconds));
    } 

    protected IEnumerator ZeroGravityRoutine(float seconds)
    {
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;

        yield return new WaitForSeconds(seconds);

        rb.gravityScale = gravity;
    }

    public virtual void BusyFor(float seconds) => StartCoroutine(BusyRoutine(seconds));

    protected IEnumerator BusyRoutine(float seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(seconds);

        isBusy = false;
    }
    #endregion

    #region Knockback
    public virtual void Knockback(Vector2 direction, float xPosition, float seconds)
    {
        if(isKnocked || !canBeKnocked )//|| !GetComponent<CharacterStats>().canBeDamaged)
            return;

        StartCoroutine(KnockbackRoutine(direction, xPosition, seconds));
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float xPosition, float seconds)
    {
        isKnocked = true;

        rb.linearVelocity = Vector2.zero;

        int knockbackDirection = xPosition > transform.position.x ? -1 : 1;
        Vector2 forceToAdd = new Vector2(direction.x * knockbackDirection, direction.y) * knockbackMultiplayer;

        rb.linearVelocity += forceToAdd;

        yield return new WaitForSeconds(seconds);

        isKnocked = false;
    }

    public void SwitchKnockability() => canBeKnocked = !canBeKnocked;

    public void EndKnockback()
    {
        isKnocked = false;
    }
    #endregion

    #region Flip
    public virtual void FlipController(float x)
    {
        if(isKnocked)
            return;

        if ((facingRight && x < 0) || (!facingRight && x > 0))
            Flip();
    }


    [ContextMenu("Flip")]
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(OnFlipped != null)
            OnFlipped();
    }
    #endregion 

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    public bool IsWallOnTheBackDetected() => Physics2D.Raycast(transform.position + (wallCheck.position - transform.position) * -1, Vector2.left * facingDir, wallCheckDistance, whatIsGround);
    
    #endregion

    public virtual void Die()
    {
        
    }

    public virtual void Stun()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
