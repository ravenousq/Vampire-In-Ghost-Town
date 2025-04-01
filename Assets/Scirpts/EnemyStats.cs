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

    protected override void Die()
    {
        base.Die();

        if(SkillManager.instance.isSkillUnlocked("Welcome To Hell"))
            PlayerManager.instance.player.ModifyBullets(bulletsToRecover);

        enemy.Die();

        if(OnDie != null)
            OnDie();
    }

}
