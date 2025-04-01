using System.Collections;
using UnityEngine;

//TODO: apply player specifix stats;
public class PlayerStats : CharacterStats
{
    private Player player;

    [Header("Player Specific")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsEnemy;

    public float iFrames; 

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();

        OnDamaged += EnableIFrames;
    }

    override protected void Update() 
    {
        base.Update();
    }

    private void EnableIFrames() => StartCoroutine(IFramesRoutine(iFrames));

    private IEnumerator IFramesRoutine(float seconds)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        canBeDamaged = false;
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        yield return new WaitForSeconds(seconds);

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        canBeDamaged = true;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        fx.IFramesFlashing(iFrames);
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }
}
