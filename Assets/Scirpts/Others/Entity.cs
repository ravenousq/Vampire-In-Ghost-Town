using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected float knockbackMultiplayer = 1;

    #region Flags
    public bool isBusy{ get; private set; }
    public bool isKnocked { get; private set; }
    public bool canBeKnocked { get; private set; } = true;
    #endregion


    public int facingDir = 1;
    public bool facingRight= true;

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
        if(isKnocked)
            Debug.Log("Is Knocked");
    }

    #region Velocity
    public void SetVelocity(Vector2 velocity)
    {
        rb.linearVelocity = velocity;

        FlipController(velocity.x);
    }

    public void SetVelocity(float x, float y)
    {
        rb.linearVelocity = new Vector2(x, y);

        FlipController(x);
    }

    public void ResetVelocity() => rb.linearVelocity = isKnocked ? rb.linearVelocity : Vector2.zero;
    #endregion

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

    public virtual void Knockback(Vector2 direction, float xPosition, float seconds)
    {
        if(isKnocked || !canBeKnocked)
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

    public void EndKnockback()
    {
        isKnocked = false;
    }

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
    }
    #endregion 

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    public bool IsWallOnTheBackDetected() => Physics2D.Raycast(transform.position + (wallCheck.position - transform.position) * -1, Vector2.left * facingDir, wallCheckDistance, whatIsGround);
    
    #endregion

    public void SwitchKnockability() => canBeKnocked = !canBeKnocked;

    public virtual void Damage()
    {
        fx.Flashing();
        //Debug.Log(gameObject.name + " recieved damage.");
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
