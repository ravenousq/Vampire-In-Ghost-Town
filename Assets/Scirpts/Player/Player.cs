using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[SelectionBase]
public class Player : Entity
{
    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idle { get; private set; }
    public PlayerMoveState move { get; private set; }
    public PlayerJumpState jump { get; private set; }
    public PlayerAirborneState airborne { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public  PlayerClimbState climb { get; private set; }
    public PlayerDashState dash { get; private set; }
    public PlayerPrimaryAttackState attack { get; private set; }
    public PlayerReloadState reload { get; private set; }
    public PlayerQuickstepState quickstep { get; private set; }
    public PlayerCrouchState crouch { get; private set; }
    public PlayerDiveState dive { get; private set; }
    public PlayerAimGunState aimGun { get; private set; }
    #endregion


    [Header("Movement")]
    public float movementSpeed;
    public float jumpForce;
    public float coyoteJumpWindow;
    public float bufferJumpWindow;
    public float wallSlideTime;
    public float wallSlideSpeed;
    public float climbSpeed;
    public float dashSpeed;
    public float dashDuration;
    public float quickstepSpeed;
    public float quickstepCooldown;
    public float diveSpeed;

    [Header("Combat")]
    public LayerMask whatIsEnemy;
    public int maxAmmo;
    public  int currentAmmo;
    public float reloadMovementSpeed;
    public float attackWindow;
    public int[] attackMovement;
    public GameObject reloadTorso;

    [Header("Abilities")]
    public SkillManager skills;
    public Crosshair crosshair { get; private set; }
    public ReapersHalo halo { get; private set; }


    [Header("Prefabs")]
    [SerializeField] private AfterImage afterImage;

    #region Flags
    [HideInInspector] public bool playStartAnim = true;
    [HideInInspector] public bool allowCoyote;
    [HideInInspector] public bool executeBuffer;
    [HideInInspector] public bool canWallSlide = true;
    [HideInInspector] public bool creatingAfterImage;
    [HideInInspector] public bool attackTrigger;
    [HideInInspector] public bool floorParry;
    [HideInInspector] public bool isAimingHalo;
    private float haloTimer;
    private bool thirdAttack;
    #endregion

    public BoxCollider2D ladderToClimb { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        idle = new PlayerIdleState(this, stateMachine, "idle");
        move  = new PlayerMoveState(this, stateMachine, "move");    
        jump = new PlayerJumpState(this, stateMachine, "jump");
        airborne = new PlayerAirborneState(this, stateMachine, "jump");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "jump");
        climb = new PlayerClimbState(this, stateMachine, "climb");
        dash = new PlayerDashState(this, stateMachine, "dash");
        attack = new PlayerPrimaryAttackState(this, stateMachine, "attack");
        reload = new PlayerReloadState(this, stateMachine, "reload");
        quickstep = new PlayerQuickstepState(this, stateMachine, "quickstep");
        crouch = new PlayerCrouchState(this, stateMachine, "crouch");
        dive = new PlayerDiveState(this, stateMachine, "jump");
        aimGun = new PlayerAimGunState(this, stateMachine, "idle");
    }

    protected override void Start()
    {
        base.Start();

        skills = SkillManager.instance;
        stateMachine.Initialize(idle);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        Reload();
    }

    protected override void Update()
    {
        stateMachine.current.Update();

        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void LateUpdate() 
    {
        CheckForDashInput();
        CheckForHaloInput();
    }

    //private void ResetMoveStart() => playStartAnim = true;

    public void TriggerLadder(BoxCollider2D ladder) => ladderToClimb = ladder;

    private void CheckForDashInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(SkillManager.instance.dash.CanUseSkill())
            {
                stateMachine.ChangeState(dash);
                creatingAfterImage = true;
                InvokeRepeating(nameof(CreateAfterImage), 0, .02f);
            }
        }
    }

    private void CheckForHaloInput()
    {
        haloTimer -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Mouse1) && !halo)
            haloTimer = 0.2f;

        if(Input.GetKeyUp(KeyCode.Mouse1) && haloTimer > 0 && skills.halo.CanUseSkill() && !halo)
        {
            skills.halo.SkipAiming();
            return;
        }

        if(haloTimer < 0 && Input.GetKey(KeyCode.Mouse1) && skills.halo.CanUseSkill() && !halo)
        {
            skills.halo.DotsActive(true);
            isAimingHalo = true;
        }

        if(!halo && Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.Mouse0) && skills.isSkillUnlocked("Bless 'em With The Blade"))
        {
            skills.halo.EnableOrbiting();
            skills.halo.DotsActive(false);
            isAimingHalo = false;
        }

        if(isAimingHalo && Input.GetKeyUp(KeyCode.Mouse1) && !halo)
        {
            isAimingHalo = false;
            SkillManager.instance.halo.CreateHalo();
        }

        if(halo && Input.GetKeyDown(KeyCode.Mouse1))
            halo.GetComponent<ReapersHalo>().StopHalo();
    }

    public void SetCrosshair(Crosshair crosshair) => this.crosshair = crosshair;

    private void CreateAfterImage()
    {
        if(!creatingAfterImage)
        {
            CancelInvoke(nameof(CreateAfterImage));
            return;
        }

        AfterImage newAfterImage = Instantiate(afterImage, transform.position, Quaternion.identity);
        newAfterImage.SetUpSprite(sr.sprite, facingRight);
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
    }

    public bool CanReload()
    {
        if(currentAmmo < maxAmmo)
            return true;
        
        return false;
    }

    public void ModifyBullets(int bullets)
    {
        currentAmmo += bullets;

        if(currentAmmo < 0)
            currentAmmo = 0;
        else if(currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;
    }

    public void ThirdAttack() => StartCoroutine(ThirdAttackRoutine());

    private IEnumerator ThirdAttackRoutine()
    {
        thirdAttack = true;

        yield return new WaitForSeconds(attackWindow);

        thirdAttack = false;
    }

    public bool CanShoot()
    {
        if(currentAmmo > 0 && !thirdAttack)
            return true;
        else if(currentAmmo > 0 && thirdAttack)
            return true;

        return false;
    }

    public bool CloseToEdge()
    {
        Vector2 edgeCheck = groundCheck.transform.position;
        int edgeToCheck = rb.linearVelocityX > 0 ? 1 : -1;

        edgeCheck += new Vector2(cd.size.x / 2 * edgeToCheck, 0);

        if(Physics2D.OverlapCircle(edgeCheck, .3f, whatIsGround))
            return false;

        return true;
    }

    public void AssignNewHalo(ReapersHalo newHalo)
    {   
        halo = newHalo;
    }
}
