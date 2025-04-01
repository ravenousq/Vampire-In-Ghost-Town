using Unity.VisualScripting;
using UnityEngine;

//TODO: apply stun mechanic;
//TODO: add on die events and apply them to skills
public class CharacterStats : MonoBehaviour
{
    public Entity entity { get; private set; }
    public FX fx { get; private set; }
    [Header("Defensive Stats")]
    [Tooltip("Determines maximum health of the character.\nRange: [1 - N/A];")]
    public Stat maxHP; 
    [Tooltip("Determines character's resistance to being stunned.\nRange: [5 - 20];")]
    public Stat poise;
    [Tooltip("Negates physical damage by 5% per point.\nRange: [0 - 20];")]
    public Stat armor;

    [Header("Offensive Stats")]
    [Tooltip("Physical damage that the character implements with it's basic attack.\nRange: [0 - N/A];")]
    public Stat damage;
    [Tooltip("Adds 5% for each point to total chance of negating enemy armor.\nRange: [0 - 20];")]
    public Stat agility;
    [Tooltip("Percentage increase of damage while attacking a stunned enemy.\nRange: [0 - 20];")]
    public Stat brutality;
    public int HP;
    [SerializeField] protected int poiseTracker;
    public System.Action OnDamaged;
    public System.Action OnStunned;
    public bool isStunned { get; private set; }
    private const int BASE_POISE_THRESHOLD = 100;
    private const int POISE_RECOVERY_RATE = 3;
    public bool canBeDamaged { get; private set; } = true;

    protected virtual void Start() 
    {
        entity = GetComponent<Entity>();
        fx = GetComponent<FX>();

        HP = maxHP.GetValue();

        poiseTracker = BASE_POISE_THRESHOLD - poise.GetValue() * 5;
        InvokeRepeating(nameof(RecoverPoise), 0, 1f);

        brutality.SetDefaultValue(5);
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " is dead fr.");
    }

    protected void RecoverPoise()
    {
        if(poiseTracker > BASE_POISE_THRESHOLD - poise.GetValue() * 5)
            poiseTracker -= POISE_RECOVERY_RATE;
        else
            poiseTracker = BASE_POISE_THRESHOLD - poise.GetValue() * 5;
    }

    public void LosePoise(int poiseToLose)
    {
        if(!canBeDamaged)
            return;

        poiseTracker += poiseToLose;

        Debug.Log(name + " lost " + poiseToLose + " poise.");

        if(poiseTracker > BASE_POISE_THRESHOLD)
        {
            isStunned = true;
            if(OnStunned != null)
                OnStunned();
        }
    }

    public virtual bool DoDamage(CharacterStats target, Vector2 knockback, float seconds, int poiseDamage = 5, float damageMultiplyer = 1)
    {
        if(target.HP <= 0 || !target.canBeDamaged)
        {
            Debug.Log(target.name + " is already dead... or is he?");
            return false;
        }

         if(!target.isStunned)
            target.LosePoise(Mathf.RoundToInt(poiseDamage * damageMultiplyer));

        int totalDamage = damage.GetValue();
        Debug.Log("Base Damage is: " + totalDamage);

        if(!CanOmitArmor() && !target.isStunned)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * (target.armor.GetValue() / 20f));
            Debug.Log("Damage after applying armor is: " + totalDamage);
        }

        if(!target.isStunned)
            target.entity.Knockback(knockback, transform.position.x, seconds);
        else
        {
            totalDamage += Mathf.RoundToInt(totalDamage * (brutality.GetValue() * .1f));
            Debug.Log("Damage after applying brutality: " + totalDamage);
        }

        target.TakeDamage(Mathf.RoundToInt(totalDamage * damageMultiplyer));

        Debug.Log(target.name + " was dealt " + Mathf.RoundToInt(totalDamage * damageMultiplyer) + " damage.");

        return true;
    }
    
    public virtual void TakeDamage(int damage)
    {
        if(!canBeDamaged)
            return;

        DecreaseHealthBy(damage);

        fx.Flashing();

        if(HP <= 0)
        {
            HP = 0;
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(int damage)
    {
        if(!canBeDamaged)
            return;

        HP -= damage;

        if(OnDamaged != null)
            OnDamaged();
    }

    //public virtual bool CanOmitArmor() => Random.Range(0, 101) <= agility.GetValue() * 5;

    public virtual bool CanOmitArmor()
    {
        if(Random.Range(0, 101) <= agility.GetValue() * 5)
        {
            Debug.Log(name + " negated enemy's armor.");
            return true;
        }

        return false;
    }

    public void CanBeDamaged(bool value) => canBeDamaged = value;
}
