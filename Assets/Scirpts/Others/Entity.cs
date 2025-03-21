using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Entity : MonoBehaviour
{
    #region Components
    public Rigidbody2D rb { get; private set; }
    public Collider2D cd { get; private set; }
    protected SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }
    #endregion

    [Header("Collision")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    #region Flags
    [HideInInspector] public bool isBusy;
    #endregion


    public int facingDir { get; private set; } = 1;
    public bool facingRight { get; private set; } = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

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

    public void ResetVelocity() => rb.linearVelocity = Vector2.zero;
    #endregion

    public void ZeroGravityFor(float seconds) => StartCoroutine(ZeroGravityRoutine(seconds));

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

    #region Flip
    public virtual void FlipController(float x)
    {
        if ((facingRight && x < 0) || (!facingRight && x > 0))
            Flip();
    }
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
    #endregion

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
