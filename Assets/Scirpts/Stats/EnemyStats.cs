using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    public System.Action OnDie;
    [Header("Enemy Specific")]
    [SerializeField] private int bulletsToRecover;

    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
    }

    public override void Die()
    {
        base.Die();

        if(SkillManager.instance.isSkillUnlocked("Welcome To Hell"))
            PlayerManager.instance.player.ModifyBullets(bulletsToRecover);

        enemy.Die();

        if(OnDie != null)
            OnDie();
    }

    protected override void Stun()
    {
        base.Stun();

        enemy.Stun();
    }

    public override void Recover()
    {
        base.Recover();

        enemy.Recover();
        Debug.Log("Recovering");
    }
}
