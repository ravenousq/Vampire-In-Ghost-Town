using UnityEngine;

public class HaloController : SkillController
{
    [Header("Die By The Blade")]
    [SerializeField] private ReapersHalo haloPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float returnSpeed;
    [SerializeField] private int numberOfBounces;
    [Header("Legend Of Steel")]
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinSpeed;
    [SerializeField] private float spinDamageWindow;
    [Header("Bless 'em With The Blade")]
    [SerializeField] private float orbitingSpeed;
    [SerializeField] private int numberOfTurns;
    [SerializeField] private float orbitDistance;

    [Header("Aim")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;
    private GameObject[] dots;
    private Vector2 finalVelocity;
    private bool skipAiming;
    private bool isOrbiting;

    public override bool CanUseSkill()
    {
        if(player.halo)
            return false;

        return base.CanUseSkill();
    }

    protected override void Start()
    {
        base.Start();
        
        GenerateDots();
    }

    override protected void Update() 
    {
        base.Update();

        if(skipAiming)
            return;
        
        if(Input.GetKeyUp(KeyCode.Mouse1))
            finalVelocity = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if(Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
        }
    }

    public void CreateHalo()
    {
        ReapersHalo newHalo = Instantiate(haloPrefab, player.transform.position, Quaternion.identity);

        newHalo.SetUpHalo(finalVelocity, player, returnSpeed, numberOfBounces, spinDuration, spinSpeed, spinDamageWindow, isOrbiting, orbitingSpeed, numberOfTurns, orbitDistance);
        DotsActive(false);

        player.AssignNewHalo(newHalo);

        skipAiming = false;
        isOrbiting = false;
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition - playerPosition;
    }

    public void DotsActive(bool isActive)
    {
        for(int i = 0; i < dots.Length; i++)
            dots[i].SetActive(isActive);
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];

        for(int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 startPosition = player.transform.position;
        Vector2 velocity = AimDirection().normalized * launchForce;
        Vector2 position = startPosition;
        Vector2 direction = velocity.normalized;

        int bounces = 0;
        int groundLayerMask = player.whatIsGround;

        while (t > 0 && bounces <= 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, direction, spaceBetweenDots, groundLayerMask);

            if (hit.collider)
            {
                direction = Vector2.Reflect(direction, hit.normal);
                position = hit.point + direction * 0.1f; 
                bounces++;
            }
            else
                position += direction * spaceBetweenDots;
            
            t -= spaceBetweenDots;
        }

        return position;
    }
    
    public void SkipAiming()
    {
        skipAiming = true;

        Transform closestEnemy = FindClosestEnemy(player.transform);

        if (closestEnemy)
            finalVelocity = (closestEnemy.position - player.transform.position).normalized * launchForce;
        else
            finalVelocity = AimDirection().normalized * launchForce;

        CreateHalo();
    }

    public void EnableOrbiting()
    {
        isOrbiting = true;
        CreateHalo();
    }
}
