using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    public System.Action OnDie;

    [Header("Level")]
    [SerializeField] private int level = 1;
    [Range(0f,1f)]
    [SerializeField] private float percentageModifier = .4f;

    [Header("Enemy Specific")]
    [SerializeField] private int bulletsToRecover;

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(maxHP);
        Modify(damage);
    }

    private void Modify(Stat stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = stat.GetValue() * percentageModifier;

            stat.AddModifier(Mathf.RoundToInt(modifier));
        }
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
